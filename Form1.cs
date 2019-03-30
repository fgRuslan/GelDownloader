﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;


namespace BooruDownloader
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bool exists = System.IO.Directory.Exists("./out/");
            if(!exists)
                System.IO.Directory.CreateDirectory("./out/");

            tagsBox.GotFocus += new EventHandler(this.TagsGotFocus);
            tagsBox.LostFocus += new EventHandler(this.TagsLostFocus);
        }

        public void TagsGotFocus(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "tags separated by whitespace")
            {
                tb.Text = "";
                tb.ForeColor = Color.Black;
            }
        }

        public void TagsLostFocus(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "tags separated by whitespace";
                tb.ForeColor = Color.DarkGray;
            }
        }

        private async void downloadButton_Click(object sender, EventArgs e)
        {
            if (isDanbooruSite.Checked){//If it's a danbooru site
                DanEngine engine = new DanEngine();
                int postCount = engine.getPostCount(domainBox.Text, tagsBox.Text);
                if (postCount == 0)
                {
                    Console.WriteLine("No posts found by tag " + tagsBox.Text);
                    statusLabel.ForeColor = Color.Red;
                    statusLabel.Text = "No posts found.";
                }
                else
                {
                    statusLabel.ForeColor = Color.Blue;
                    statusLabel.Text = "Downloading...";
                    for (int i = 0; i < postCount; i++)
                    {
                        await Task.Run(() => engine.downloadPosts(domainBox.Text, tagsBox.Text, i, checkBox1.Checked));
                    }
                    statusLabel.ForeColor = Color.Green;
                    statusLabel.Text = "Ready.";
                    MessageBox.Show("Download compelete!", "BooruDownloader", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                }
            }
            else
            {//If it's a gelbooru site

                GelEngine engine = new GelEngine();
                int postCount = engine.getPostCount(domainBox.Text, tagsBox.Text);
                if (postCount == 0)
                {
                    Console.WriteLine("No posts found by tag " + tagsBox.Text);
                    statusLabel.ForeColor = Color.Red;
                    statusLabel.Text = "No posts found.";
                }
                else
                {
                    statusLabel.ForeColor = Color.Blue;
                    statusLabel.Text = "Downloading...";
                    for (int i = 0; i < postCount; i++)
                    {
                        await Task.Run(() => engine.downloadPosts(domainBox.Text, tagsBox.Text, i, checkBox1.Checked));
                    }
                    statusLabel.ForeColor = Color.Green;
                    statusLabel.Text = "Ready.";
                    MessageBox.Show("Download compelete!", "BooruDownloader", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                }

            }
        }
    }
}
