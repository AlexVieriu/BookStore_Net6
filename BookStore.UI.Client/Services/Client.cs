﻿namespace BookStore.UI.Client.Services.Base;

public partial class Client : IClient
{
    public HttpClient HttpClient
    {
        get
        {
            return _httpClient;
        }
    }
}
