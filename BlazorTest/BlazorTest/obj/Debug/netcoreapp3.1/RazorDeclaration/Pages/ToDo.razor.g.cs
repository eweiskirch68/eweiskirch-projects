// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace BlazorTest.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\Student\workspace\personal-projects\BlazorTest\BlazorTest\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Student\workspace\personal-projects\BlazorTest\BlazorTest\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Student\workspace\personal-projects\BlazorTest\BlazorTest\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Student\workspace\personal-projects\BlazorTest\BlazorTest\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Student\workspace\personal-projects\BlazorTest\BlazorTest\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Student\workspace\personal-projects\BlazorTest\BlazorTest\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\Student\workspace\personal-projects\BlazorTest\BlazorTest\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\Student\workspace\personal-projects\BlazorTest\BlazorTest\_Imports.razor"
using BlazorTest;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\Student\workspace\personal-projects\BlazorTest\BlazorTest\_Imports.razor"
using BlazorTest.Shared;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Components.RouteAttribute("/todo")]
    public partial class ToDo : global::Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 21 "C:\Users\Student\workspace\personal-projects\BlazorTest\BlazorTest\Pages\ToDo.razor"
       
    private List<string> Todos = new List<string>();

    private List<DateTime?> DueDates = new List<DateTime?>();

    private string NewTodo = string.Empty;

    private DateTime? DueDate = null;

    private void Save()
    {
        if (string.IsNullOrEmpty(NewTodo)) return;

        Todos.Add(NewTodo);
        NewTodo = string.Empty;

        DueDates.Add(DueDate);
        DueDate = null;
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591