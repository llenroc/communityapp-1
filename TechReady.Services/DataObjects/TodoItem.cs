using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core;
using System.ServiceModel.Syndication;
using Microsoft.WindowsAzure.Mobile.Service;
using Newtonsoft.Json;

namespace TechReady.Services.DataObjects
{
    public class TodoItem : EntityData
    {
        public string Text { get; set; }

        public bool Complete { get; set; }
    }




}