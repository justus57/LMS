﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OshoPortal
{
    //public class GridView
    //{
    //    MVCGridDefinitionTable.Add("TestGrid", new MVCGridBuilder<Person>(colDefauls)
    //.WithAuthorizationType(AuthorizationType.AllowAnonymous)
    //.WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
    //.WithPaging(paging: true, itemsPerPage: 10, allowChangePageSize: true, maxItemsPerPage: 100)
    //.WithAdditionalQueryOptionNames("search")
    //.AddColumns(cols =>
    //{
    //        cols.Add("Id").WithValueExpression((p, c) => c.UrlHelper.Action("detail", "demo", new { id = p.Id }))
    //            .WithValueTemplate("<a href='{Value}'>{Model.Id}</a>", false)
    //            .WithPlainTextValueExpression(p => p.Id.ToString());
    //        cols.Add("FirstName").WithHeaderText("First Name")
    //            .WithVisibility(true, true)
    //            .WithValueExpression(p => p.FirstName);
    //        cols.Add("LastName").WithHeaderText("Last Name")
    //            .WithVisibility(true, true)
    //            .WithValueExpression(p => p.LastName);
    //        cols.Add("FullName").WithHeaderText("Full Name")
    //            .WithValueTemplate("{Model.FirstName} {Model.LastName}")
    //            .WithVisibility(visible: false, allowChangeVisibility: true)
    //            .WithSorting(false);
    //        cols.Add("StartDate").WithHeaderText("Start Date")
    //            .WithVisibility(visible: true, allowChangeVisibility: true)
    //            .WithValueExpression(p => p.StartDate.HasValue ? p.StartDate.Value.ToShortDateString() : "");
    //        cols.Add("Status")
    //            .WithSortColumnData("Active")
    //            .WithVisibility(visible: true, allowChangeVisibility: true)
    //            .WithHeaderText("Status")
    //            .WithValueExpression(p => p.Active ? "Active" : "Inactive")
    //            .WithCellCssClassExpression(p => p.Active ? "success" : "danger");
    //        cols.Add("Gender").WithValueExpression((p, c) => p.Gender)
    //            .WithAllowChangeVisibility(true);
    //        cols.Add("Email")
    //            .WithVisibility(visible: false, allowChangeVisibility: true)
    //            .WithValueExpression(p => p.Email);
    //        cols.Add("Url").WithVisibility(false)
    //            .WithValueExpression((p, c) => c.UrlHelper.Action("detail", "demo", new { id = p.Id }));
    //    })
    ////.WithAdditionalSetting(MVCGrid.Rendering.BootstrapRenderingEngine.SettingNameTableClass, "notreal") // Example of changing table css class
    //.WithRetrieveDataMethod((context) =>
    //{
    //        var options = context.QueryOptions;
    //        int totalRecords;
    //        var repo = DependencyResolver.Current.GetService<IPersonRepository>();
    //        string globalSearch = options.GetAdditionalQueryOptionString("search");
    //        string sortColumn = options.GetSortColumnData<string>();
    //        var items = repo.GetData(out totalRecords, globalSearch, options.GetLimitOffset(), options.GetLimitRowcount(),
    //            sortColumn, options.SortDirection == SortDirection.Dsc);
    //        return new QueryResult<Person>()
    //        {
    //            Items = items,
    //            TotalRecords = totalRecords
    //        };
    //    }));
    //}
}