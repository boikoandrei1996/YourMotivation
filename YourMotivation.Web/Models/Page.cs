﻿using System.Collections.Generic;

namespace YourMotivation.Web.Models
{
  public class Page<T> where T : class, new()
  {
    public Page()
    {
      Records = new List<T>();
    }

    public Page(IEnumerable<T> records)
    {
      Records = new List<T>(records);
    }

    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public List<T> Records { get; set; }
    public string StatusMessage { get; set; }
    public T Default { get { return new T(); } }
  }
}