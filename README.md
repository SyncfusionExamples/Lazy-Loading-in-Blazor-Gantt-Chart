# Load on Demand: A Solution for Efficiently Handling Large Datasets in Blazor Gantt Chart  
The Syncfusion Blazor Gantt Chart allows you to render DOM elements effectively using virtualization support in the default remote dataset. We have already discussed boosting performance with virtualization in the blog below.

https://www.syncfusion.com/blogs/post/boosting-performance-of-blazor-gantt-chart-using-virtualization.aspx 

Yet it takes all the records from the dataset and validates all the available data during load time. Hence, it takes more time when you have a large dataset, say more than 100,000 records. We have now added "lazy loading" support in the 2022 Volume 4 release, allowing you to load more than 100,000 records with effective performance in Gantt Chart.
Using lazy loading concept, it fetches only the required number of parent records from the remote service on initial load, and remaining records are fetched on demand, when expanding or scrolling the Gantt Chart.

## Lazy loading (load on demand) in Blazor Gantt Chart
In the Syncfusion Blazor Gantt Chart, **SfDataManager** sends a request to fetch only the root parent records from the remote service in a collapsed state with the help of [GanttTaskFields.HasChildMapping](https://help.syncfusion.com/cr/blazor/Syncfusion.Blazor.Gantt.GanttTaskFields.html#Syncfusion_Blazor_Gantt_GanttTaskFields_HasChildMapping) property. It allows you both Virtualization enabled and disabled of Gantt Chart.
### Virtualization disabled
It fetches all root parent records, and the child records are lazy loaded only when expanding the parent record. But it may still have performance problems when you have a large number of root parent records.
### Virtualization enabled
It fetches the necessary root parent for the current view port. While scrolling the parent record, on demand, it fetches the immediate child records. During scrolling, the next set of records is fetched from the web service.

You can find the real-time difference in data load time in the metrics below: 
Record Count |With lazy loading | Without lazy loading
----- |----- |-----
50,000 |0.82 seconds |4 minutes 15 seconds
75,000 |0.82 seconds |8 minutes 25 seconds
1,00,000	|0.82 seconds |18 minutes 45 seconds

`Note:` Above metrics may vary depend on machine performance and RAM free space.

### Reference
For more details, refer to the [Load on demand in Blazor Gantt Chart in live demo](https://blazor.syncfusion.com/demos/gantt-chart/load-on-demand) and [documentation](https://blazor.syncfusion.com/documentation/gantt-chart/data-binding#load-child-on-demand).
