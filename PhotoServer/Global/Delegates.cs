using System;

namespace PhotoServer
{
    public delegate void VoidDelegate();
    public delegate void PhotoShootDelegate(PhotoShoot.PhotoShoot photoShoot);
    public delegate void StringStringDelegate(string str1, string str2);
}
