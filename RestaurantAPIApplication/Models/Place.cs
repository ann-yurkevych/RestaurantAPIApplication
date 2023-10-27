using System;
using System.Collections.Generic;

namespace RestaurantAPIApplication.Models;

public partial class Place
{
    public int Id { get; set; }

    public int TypeId { get; set; }

    public string Name { get; set; }

    public double AverageBill { get; set; }

    public string OpenTime { get; set; }

    public string CloseTime { get; set; }

    public string Location { get; set; } = null!;

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

    public virtual Type Type { get; set; } = null!;
}
