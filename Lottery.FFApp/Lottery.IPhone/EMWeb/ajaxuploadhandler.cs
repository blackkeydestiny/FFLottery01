﻿// Decompiled with JetBrains decompiler
// Type: Lottery.EMWeb.ajaxuploadhandler
// Assembly: Lottery.IPhone, Version=1.0.1.1, Culture=neutral, PublicKeyToken=null
// MVID: 6A500EAD-7331-41B6-AA69-9D37BCA394DC
// Assembly location: F:\pros\tianheng\bf\Iphone\bin\Lottery.IPhone.dll

using System;
using System.IO;
using System.Web;

namespace Lottery.EMWeb
{
  public class ajaxuploadhandler : IHttpHandler
  {
    public void ProcessRequest(HttpContext context)
    {
      context.Response.ContentType = "text/plain";
      HttpPostedFile file = context.Request.Files[0];
      string path1 = "/statics/temp/";
      int contentLength = file.ContentLength;
      int num = 512000;
      string s = "-1";
      if (contentLength <= num)
      {
        byte[] buffer = new byte[contentLength];
        file.InputStream.Read(buffer, 0, contentLength);
        MemoryStream memoryStream = new MemoryStream(buffer);
        string path2 = HttpContext.Current.Server.MapPath(path1);
        if (!Directory.Exists(path2))
          Directory.CreateDirectory(path2);
        string str = ajaxuploadhandler.CreateIdCode() + ".txt";
        file.SaveAs(path2 + str);
        s = path1 + str;
        memoryStream.Close();
      }
      context.Response.Write(s);
    }

    public static string CreateIdCode()
    {
      return (DateTime.Now.ToUniversalTime() - Convert.ToDateTime("1970-01-01")).TotalMilliseconds.ToString("0");
    }

    public bool IsReusable
    {
      get
      {
        throw new NotImplementedException();
      }
    }
  }
}
