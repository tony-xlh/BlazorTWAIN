using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTWAIN.Client
{
    public class DWT
    {
        private Action<string> base64Action;
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

        [JSInvokable("BlazorTWAIN.Client")]
        public void RetrieveBase64(string name, string base64)
        {
            base64Action.Invoke(base64);
        }

        public async Task GetBase64OfSelected(Action<string> action)
        {
            base64Action = action;
            await module.InvokeVoidAsync("GetBase64OfSelected", DotNetObjectReference.Create(this));
        }

        protected virtual async ValueTask DisposeAsync()
        {
            await module.DisposeAsync();
        }
    }
}
