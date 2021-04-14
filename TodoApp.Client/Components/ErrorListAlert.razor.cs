using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Client.Components
{
    public partial class ErrorListAlert
    {
        [Parameter]
        public List<string> Errors { get; set; }
    }
}
