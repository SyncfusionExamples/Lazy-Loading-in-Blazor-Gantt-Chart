using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using WebAPI.Data;
using Syncfusion.Blazor.Data;
using System.Collections;

namespace WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {

        public static List<TaskData> DataList = new List<TaskData>();
        private static DataRequest req = null;
        [HttpGet]
        public async Task<object> Get(int? code)
        {
            DataList.Clear();
            IQueryCollection queryString = Request.Query;
            req = QueryGenerator(queryString);
            if (TaskData.tree.Count == 0)
                TaskData.GetTree();
            if (req.filter != "" && !req.filter.Contains("null"))
            {
                int fltr = Int32.Parse(req.filter.Split("eq")[1]);
                IQueryable<TaskData> data1 = TaskData.tree.Where(f => f.ParentId == fltr).AsQueryable();
                if (queryString.Keys.Contains("$orderby"))
                {
                    string srt;
                    srt = req.orderby.Replace("desc", "descending");
                    data1 = SortingExtend.Sort(data1, srt);
                }
                return new { result = data1.ToList(), items = data1.ToList(), count = data1.Count() };
            }
            List<TaskData> data = TaskData.tree.ToList();
            if (req.orderby != "")
            {
                string srt;
                srt = req.orderby.Replace("desc", "descending");
                IQueryable<TaskData> data1 = SortingExtend.Sort(data.AsQueryable(), srt);
                data = data1.ToList();
            }
            if (queryString.Keys.Contains("$select"))
            {
                data = (from rec in TaskData.tree
                        select new TaskData
                        {
                            ParentId = rec.ParentId
                        }
                        ).ToList();
                return data;
            }
            data = data.Where(p => p.ParentId == null).ToList();
            int count = data.Count;
            if (req.inlinecount)
            {
                if (req.skip == null && req.take == null)
                    DataList = data;
                else
                    DataList = data.Skip((int)req.skip).Take((int)req.take).ToList();
                if (req.loadchild)
                {
                    var GroupData = TaskData.tree.ToList().GroupBy(rec => rec.ParentId)
                                .Where(g => g.Key != null).ToDictionary(g => g.Key?.ToString(), g => g.ToList());
                    foreach (var Record in DataList.ToList())
                    {
                        if (GroupData.ContainsKey(Record.ID.ToString()))
                        {
                            var ChildGroup = GroupData[Record.ID.ToString()];
                            if (ChildGroup?.Count > 0)
                                AppendChildren(ChildGroup, Record, GroupData);
                        }
                    }
                }
                if (req.skip == null && req.take == null)
                    return new { result = DataList, items = DataList, count = count };
                return new { result = DataList, items = DataList, count = count };
            }
            else
            {
                return TaskData.GetTree();
            }
        }

        // This method is used to generate the query. Based on the query values records are fetched.
        public DataRequest QueryGenerator(IQueryCollection queryString)
        {
            DataRequest req = new DataRequest();
            StringValues Skip;
            StringValues Take;
            StringValues filter;
            StringValues orderby;
            StringValues loadchild;
            req.loadchild = queryString.TryGetValue("loadchildondemand", out loadchild) ? Convert.ToBoolean(loadchild[0]) : false;
            req.skip = queryString.TryGetValue("$skip", out Skip) ? Convert.ToInt32(Skip[0]) : (Nullable<int>)null;
            req.take = (queryString.TryGetValue("$top", out Take)) ? Convert.ToInt32(Take[0]) : (Nullable<int>)null;
            req.filter = queryString.TryGetValue("$filter", out filter) ? filter[0].ToString() : "";
            req.inlinecount = queryString.Keys.Contains("$inlinecount") ? true : false;
            req.orderby = queryString.TryGetValue("$orderby", out orderby) ? orderby[0].ToString() : "";
            return req;
        }

        // This method is used to fetch child records also on load time when LoadChildOnDemand is enabled.
        private void AppendChildren(List<TaskData> ChildRecords, TaskData ParentItem, Dictionary<string, List<TaskData>> GroupData)
        {
            var queryString = Request.Query;
            string TaskId = ParentItem.ID.ToString();
            if (queryString.Keys.Contains("$orderby"))
            {
                StringValues srt;
                queryString.TryGetValue("$orderby", out srt);
                srt = srt.ToString().Replace("desc", "descending");
                List<TaskData> SortedChildRecords = SortingExtend.Sort(ChildRecords.AsQueryable(), srt).ToList();
                var index = DataList.IndexOf(ParentItem);
                foreach (var Child in SortedChildRecords)
                {
                    string ParentId = Child.ParentId.ToString();
                    if (TaskId == ParentId)
                    {
                        if (DataList.IndexOf(Child) == -1)
                            ((IList)DataList).Insert(++index, Child);
                        if (GroupData.ContainsKey(Child.ID.ToString()))
                        {
                            var DeepChildRecords = GroupData[Child.ID.ToString()];
                            if (DeepChildRecords?.Count > 0)
                                AppendChildren(DeepChildRecords, Child, GroupData);
                        }
                    }
                }
            }
            else
            {
                var index = DataList.IndexOf(ParentItem);
                foreach (var Child in ChildRecords)
                {
                    string ParentId = Child.ParentId.ToString();
                    if (TaskId == ParentId)
                    {
                        if (DataList.IndexOf(Child) == -1)
                            ((IList)DataList).Insert(++index, Child);
                        if (GroupData.ContainsKey(Child.ID.ToString()))
                        {
                            var DeepChildRecords = GroupData[Child.ID.ToString()];
                            if (DeepChildRecords?.Count > 0)
                                AppendChildren(DeepChildRecords, Child, GroupData);
                        }
                    }
                }
            }
        }
        public class DataRequest
        {
            public Nullable<int> skip { get; set; }
            public Nullable<int> take { get; set; }
            public Boolean inlinecount { get; set; }
            public string filter { get; set; }
            public string orderby { get; set; }
            public bool loadchild { get; set; }
            public DateTime startDate { get; set; }
            public DateTime endDate { get; set; }
        }
    }
    public static class SortingExtend
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> source, string sortBy)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (string.IsNullOrEmpty(sortBy))
                throw new ArgumentNullException("sortBy");
            source = (IQueryable<T>)source.OrderBy(sortBy);
            return source;
        }
    }
}