# Load on Demand: A Solution for Efficiently Handling Large Datasets in Blazor Gantt Chart

The [Blazor Gantt Chart](https://www.syncfusion.com/blazor-components/blazor-gantt-chart?utm_source=github&utm_medium=listing&utm_campaign=blazor-gantt-chart-github-samples) is a project planning and management tool that provides a Microsoft Project-like interface to display and manage hierarchical tasks with timeline details. 

The Syncfusion Blazor Gantt Chart supports efficient rendering of large datasets through virtualization when working with remote data sources. Virtualization minimizes DOM creation by rendering only the records visible in the viewport, significantly improving performance for large datasets.

For an introduction to virtualization, refer to the blog below:
https://www.syncfusion.com/blogs/post/boosting-performance-of-blazor-gantt-chart-using-virtualization.aspx 

However, even with virtualization enabled, the Gantt Chart traditionally retrieved and validated all records at load time. When working with very large datasets (for example, more than 100,000 records), this initial data processing could still result in long load times.

To address this, **lazy loading (Load Child on Demand)** support was introduced in the 2022 Volume 4 release of the Syncfusion Blazor Gantt Chart. This enhancement enables the Gantt Chart to handle extremely large datasets efficiently by fetching data only when required.

## Lazy loading (load on demand) in Blazor Gantt Chart

In the Syncfusion Blazor Gantt Chart, **SfDataManager** sends a request to fetch only the root parent records from the remote service in a collapsed state with the help of [GanttTaskFields.HasChildMapping](https://help.syncfusion.com/cr/blazor/Syncfusion.Blazor.Gantt.GanttTaskFields.html#Syncfusion_Blazor_Gantt_GanttTaskFields_HasChildMapping) property. It allows you both Virtualization enabled and disabled of Gantt Chart.

### Virtualization disabled

- All root parent tasks are fetched during the initial load.
- Child tasks are retrieved only when a parent task is expanded.
- While this improves performance compared to loading the entire dataset, it may still be inefficient if the number of root parent records is very large.

### Virtualization enabled
- Only the root parent tasks required for the current viewport are fetched initially.
- As the user scrolls, additional parent tasks are loaded dynamically.
- Child tasks are fetched on demand when expanding parent tasks.
- This approach provides the best performance for extremely large datasets by minimizing both data transfer and DOM rendering.

## Performance comparison

You can find the real-time difference in data load time in the metrics below: 
Record Count |With lazy loading | Without lazy loading
----- |----- |-----
50,000 |0.82 seconds |4 minutes 15 seconds
75,000 |0.82 seconds |8 minutes 25 seconds
1,00,000	|0.82 seconds |18 minutes 45 seconds

`Note:` Above metrics may vary depend on machine performance and RAM free space.

## Prerequisites

- Visual Studio 2022 (or later)
- .NET SDK 8.0 or later
- Syncfusion Blazor Gantt NuGet package
- A valid Syncfusion license (Community or Trial)

## How to run the project

- Clone or download this repository to your local system.
- Open the project file (.csproj) in Visual Studio 2022 or later.
- Restore the required NuGet packages.
- Build the project to ensure there are no compilation errors.
- Run the application.
- Navigate to the page hosting the Gantt Chart and explore lazy loading.

## Reference

- [Live Demo- Load on Demand](https://blazor.syncfusion.com/demos/gantt-chart/load-on-demand) 
- [Documentation - Load Child on Demand](https://blazor.syncfusion.com/documentation/gantt-chart/data-binding#load-child-on-demand).

## Related links

[Learn More about Blazor Gantt Chart](https://www.syncfusion.com/blazor-components/blazor-gantt-chart?utm_source=github&utm_medium=listing&utm_campaign=blazor-gantt-chart-github-samples) <br/><br/>
[Download Free Trial](https://www.syncfusion.com/downloads?utm_source=github&utm_medium=listing&utm_campaign=blazor-gantt-chart-github-samples) <br/><br/>
[Pricing](https://www.syncfusion.com/sales/products/blazor?utm_source=github&utm_medium=listing&utm_campaign=blazor-gantt-chart-github-samples) <br/><br/>
[Documentation](https://blazor.syncfusion.com/documentation/gantt-chart/getting-started?utm_source=github&utm_medium=listing&utm_campaign=blazor-gantt-chart-github-samples) <br/><br/>
[Online Examples](https://blazor.syncfusion.com/demos/gantt-chart/default-functionalities?utm_source=github&utm_medium=listing&utm_campaign=blazor-gantt-chart-github-samples) <br/><br/>
[Watch a How-to Video](https://www.syncfusion.com/tutorial-videos/blazor/gantt-chart?title=create-a-gantt-chart-component-in-a-blazor-webassembly) <br/><br/>
[Community Forums](https://www.syncfusion.com/forums/blazor-components/gantt-chart?utm_source=github&utm_medium=listing&utm_campaign=blazor-gantt-chart-github-samples) <br/><br/>
[Suggest a feature](https://www.syncfusion.com/feedback/blazor-components?utm_source=github&utm_medium=listing&utm_campaign=blazor-gantt-chart-github-samples)

## Syncfusion License

This sample uses the Syncfusion Blazor components, which require a valid Syncfusion license.

- Community License: https://www.syncfusion.com/products/communitylicense
- Trial License: https://www.syncfusion.com/account/manage-trials/start-trials

Ensure the license key is registered before running the application.
