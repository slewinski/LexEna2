using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LexEnaTrs.Views
{
    public partial class DispHtmlContent : UserControl
    {
        public DispHtmlContent()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RadHtmlPlaceholder1.HtmlSource = @"You can display <b>any</b> <span style=""color:#FF0000;"">valid</span>
<i>html</i> content.<br/>It will be displayed as part of the <a href=""http://silverlight.net"" 
target=""_blank"">Silverlight</a> page<br/> and will be rendered by the browser.";
        }
    }
}
