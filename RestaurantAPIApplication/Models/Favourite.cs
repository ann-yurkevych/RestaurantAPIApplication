using System;
using System.Collections.Generic;

namespace RestaurantAPIApplication.Models;

public partial class Favourite
{
    public int Id { get; set; }

    public int PlaceId { get; set; }

    public int ClientId { get; set; }

    public virtual Client Client { get; set; }

    public virtual Place Place { get; set; }
}
