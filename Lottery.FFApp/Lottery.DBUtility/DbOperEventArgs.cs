﻿// Decompiled with JetBrains decompiler
// Type: Lottery.DBUtility.DbOperEventArgs
// Assembly: Lottery.DBUtility, Version=1.0.1.1, Culture=neutral, PublicKeyToken=null
// MVID: 41391965-66A5-4DE4-8203-13B298F4A572
// Assembly location: F:\pros\tianheng\bf\WebAppOld\bin\Lottery.DBUtility.dll

using System;

namespace Lottery.DBUtility
{
  public class DbOperEventArgs : EventArgs
  {
    public int id;

    public DbOperEventArgs(int _id)
    {
      this.id = _id;
    }
  }
}
