﻿using ThunderHerd.Core.Enums;
using ThunderHerd.Core.Models.Settings;

namespace ThunderHerd.Domain.Interfaces
{
    public interface IHerdClient
    {
        Task<HttpResponseMessage> SendAsync(string url, HttpMethods method = HttpMethods.GET, HerdClientRequestSettings? settings = default, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> SendAsync(Uri uri, HttpMethods method = HttpMethods.GET, HerdClientRequestSettings? settings = default, CancellationToken cancellationToken = default);
    }
}
