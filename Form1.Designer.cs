using System.Collections.Specialized;
using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Resources;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace StainedGlassGenerator{

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        string imgsResourceFile = @".\imgs.resx";
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(240, 110);
        this.Text = "彩色玻璃生成器";
        using (ResXResourceSet resxSet = new ResXResourceSet(imgsResourceFile)){
            this.Icon = (Icon)resxSet.GetObject("logo.ico");
        }
        this.drawOriginBtn = new System.Windows.Forms.Button();
        this.doneBtn = new System.Windows.Forms.Button();
        this.spacingLabel = new System.Windows.Forms.Label();
        this.spacingText = new System.Windows.Forms.TextBox();
        this.spacingBar = new System.Windows.Forms.TrackBar();
        this.seedLabel = new System.Windows.Forms.Label();
        this.seedText = new System.Windows.Forms.TextBox();

        // 
        // drawOriginBtn
        // 
        this.drawOriginBtn.Location = new System.Drawing.Point(0, 90);
        this.drawOriginBtn.Name = "drawOriginBtn";
        this.drawOriginBtn.Size = new System.Drawing.Size(80, 20);
        this.drawOriginBtn.Text = "绘制原图";
        this.drawOriginBtn.UseVisualStyleBackColor = true;
        this.drawOriginBtn.Click += new System.EventHandler(this.DrawOrigin);

        // 
        // doneBtn
        // 
        this.doneBtn.Location = new System.Drawing.Point(90, 90);
        this.doneBtn.Name = "doneBtn";
        this.doneBtn.Size = new System.Drawing.Size(80, 20);
        this.doneBtn.Text = "生成玻璃";
        this.doneBtn.UseVisualStyleBackColor = true;
        this.doneBtn.Click += new System.EventHandler(this.DyeGlasses);

        // 
        // spacingLabel
        // 
        this.spacingLabel.Location = new System.Drawing.Point(4, 4);
        this.spacingLabel.Name = "spacingLabel";
        this.spacingLabel.Size = new System.Drawing.Size(120, 16);
        this.spacingLabel.Text = "玻璃碎片尺寸(像素):";

        // 
        // spacingText
        // 
        this.spacingText.Font = new System.Drawing.Font("宋体", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.spacingText.Location = new System.Drawing.Point(124, 0);
        this.spacingText.Name = "spacingText";
        this.spacingText.Size = new System.Drawing.Size(50, 20);
        this.spacingText.Text = "32";
        this.spacingText.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
        this.spacingText.TextChanged += new System.EventHandler(this.SpacingTextChanged);

        // 
        // spacingBar
        // 
        this.spacingBar.Location = new System.Drawing.Point(0, 20);
        this.spacingBar.Name = "spacingBar";
        this.spacingBar.Size = new System.Drawing.Size(240, 20);
        this.spacingBar.Minimum = 8;
        this.spacingBar.Maximum = 320;
        this.spacingBar.TickFrequency = 8;
        this.spacingBar.LargeChange = 8;
        this.spacingBar.SmallChange = 4;
        this.spacingBar.Value = 32;
        this.spacingBar.Scroll += new System.EventHandler(this.SpacingBarScroll);

        // 
        // seedLabel
        // 
        this.seedLabel.Location = new System.Drawing.Point(4, 70);
        this.seedLabel.Name = "seedLabel";
        this.seedLabel.Size = new System.Drawing.Size(60, 16);
        this.seedLabel.Text = "随机种子:";

        // 
        // seedText
        // 
        this.seedText.Font = new System.Drawing.Font("宋体", 8.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.seedText.Location = new System.Drawing.Point(64, 66);
        this.seedText.Name = "seedText";
        this.seedText.Size = new System.Drawing.Size(50, 20);
        this.seedText.Text = "777";
        this.seedText.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
        this.seedText.TextChanged += new System.EventHandler(this.SeedTextChanged);

        this.Controls.Add(drawOriginBtn);
        this.Controls.Add(doneBtn);
        this.Controls.Add(spacingLabel);
        this.Controls.Add(spacingText);
        this.Controls.Add(spacingBar);
        this.Controls.Add(seedLabel);
        this.Controls.Add(seedText);
        this.drawOriginBtn.TabStop = false;
        this.doneBtn.TabStop = false;
        this.spacingLabel.TabStop = false;
        this.spacingText.TabStop = false;
        this.spacingBar.TabStop = false;
        this.seedLabel.TabStop = false;
        this.seedText.TabStop = false;
    }

    #endregion
    private System.Windows.Forms.Button drawOriginBtn;
    private System.Windows.Forms.Button doneBtn;
    private System.Windows.Forms.Label spacingLabel;
    private System.Windows.Forms.TextBox spacingText;
    private System.Windows.Forms.TrackBar spacingBar;
    private System.Windows.Forms.Label seedLabel;
    private System.Windows.Forms.TextBox seedText;
}

}
