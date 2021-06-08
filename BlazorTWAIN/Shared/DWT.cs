using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTWAIN.Shared
{
    public class DWT
    {
        private IJSObjectReference module;
        [Inject]
        IJSRuntime JS { get; set; }
        private DWT()
        {            
        }
        public static async Task<DWT> CreateAsync(IJSRuntime JS)
        {
            DWT dwt = new DWT();
            dwt.JS = JS;
            await dwt.LoadJSAsync();
            return dwt;
        }

        private async Task LoadJSAsync()
        {
            module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/DWT.js");
        }

        public async Task CreateDWT()
        {
            await module.InvokeVoidAsync("CreateDWT");
        }

        public async Task Scan()
        {
            await module.InvokeVoidAsync("Scan");
        }

        public async Task LoadImage()
        {
            await module.InvokeVoidAsync("LoadImage");
        }

        public async Task Save()
        {
            await module.InvokeVoidAsync("Save");
        }

        public async Task<Boolean> IsDesktop()
        {
            return await module.InvokeAsync<Boolean>("isDesktop");
        }

        protected virtual async ValueTask DisposeAsync()
        {
            await module.DisposeAsync();
        }
    }
}
