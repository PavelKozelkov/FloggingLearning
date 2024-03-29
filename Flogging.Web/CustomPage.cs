﻿using Flogging.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flogging.Web
{
    public abstract class CustomPage : System.Web.UI.Page
    {
        protected PerfTracker Tracker;

        protected override void OnLoad(EventArgs e)
        {
            var name = Page.Request.Path + (IsPostBack ? "_POSTBACK" : "");

            string userId, userName, location;
            var data = Helpers.GetWebFloggingData(out userId, out userName, out location);

            Tracker = new PerfTracker(name, userId, userName, location, "ToDos", "WebForms", data);
            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            if (Tracker != null) Tracker.Stop();
        }
    }
}
