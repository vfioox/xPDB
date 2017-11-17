using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using xPDB.Models.Objects;
using xPDB.Models.Storage;
using xPDB.Network;
using xPDB.Properties;
using xPDB.Storage;
using xPDB.Utility;
using xPDB.Windows.Helpers;

namespace xPDB
{
    public partial class MainWindow
    {

        

        #region _8chan

        public async void load8Boards()
        {
            spiderWork.Start();
            await Task.Run(() =>
            {
                var boards = new _8chanGetter().getBoards();
                if (boards != null)
                {
                    this.Invoke((MethodInvoker)(() =>
                    {
                        _8ChanBoardsOListView.SetObjects(boards);
                    }));
                }
            });
            spiderWork.Stop();
        }

        #endregion _8chan

        #region deviantart

        private string SavedToken;

        void SaveToken(string newRefreshToken, DateTime x)
        {
            this.SavedToken = newRefreshToken;
            if (cm.cfg.OStorage.ContainsKey("dac"))
            {
                var dac = cm.cfg.OStorage["dac"] as DACredentials;
                dac.Token = newRefreshToken;
                cm.cfg.OStorage["dac"] = dac;
            }
        }

        void RefreshTokenUpdated(string newRefreshToken)
        {
            if (SavedToken != newRefreshToken)
                SaveToken(newRefreshToken, DateTime.Now.AddMonths(3));
        }

        private void DACustomSignInAsync()
        {
            
        }
        public async void loadDA()
        {
            if (cm.cfg.OStorage.ContainsKey("dac"))
            {
                var dac = cm.cfg.OStorage["dac"] as DACredentials;
                if (dac != null)
                {
                    textBox2.Text = dac.ClientId;
                    textBox3.Text = dac.Secret;
                    SavedToken = dac.Token;
                }
            }
            var ClientId = textBox2.Text;
            var Secret = textBox3.Text;
            Login.CustomSignInAsync = delegate
            {

                return null;
            };
            var scopes = new[]
            {
                DeviantartApi.Login.Scope.Browse,
                DeviantartApi.Login.Scope.User,
                DeviantartApi.Login.Scope.Feed
            };
            var result = await DeviantartApi.Login.SignInAsync(ClientId, Secret, "xpdb://dabegin", RefreshTokenUpdated, scopes);

            var tvd = new TreeViewDisplay(result, ref cm);
            tvd.Show();
            return;
        }

        #endregion deviantart
    }
}
