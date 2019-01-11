using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace XeonComputers.Services.Contracts
{
    public interface IViewRender
    {
        string Render<TModel>(string name, TModel model);
    }
}