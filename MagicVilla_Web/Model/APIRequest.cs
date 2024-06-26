﻿using Microsoft.AspNetCore.Mvc;
using static MagicVilla_Utility.SD;

namespace MagicVilla_Web.Model
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string token { get; set; }
        public ContentType ContentType { get; set; } = ContentType.Json;
    }
}
