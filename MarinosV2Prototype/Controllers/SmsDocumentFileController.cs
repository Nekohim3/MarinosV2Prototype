﻿using MarinosV2Prototype.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarinosV2Prototype.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SmsDocumentFileController : TController<SmsDocumentFile>
    {
        public SmsDocumentFileController(MarinosContext ctx) : base(ctx)
        {
        }
    }
}