using System;
using System.Runtime.InteropServices;
using UnityEngine;

public enum ERetainPolicy
{
    Retained,
    Unretained
}

public class UnmanagedObject : object
{
    internal HandleRef Handle { get; set; }

    internal readonly ERetainPolicy _retainPolicy;
    internal UnmanagedObject() { }

    public UnmanagedObject(IntPtr ptr)
    {
        _retainPolicy = ERetainPolicy.Retained;
        Handle = new HandleRef(this, ptr);
    }

    public UnmanagedObject(IntPtr ptr, ERetainPolicy retainPolicy)
    {
        _retainPolicy = retainPolicy;
        Handle = new HandleRef(this, ptr);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return Equals(obj as UnmanagedObject);
    }

    public bool Equals(UnmanagedObject rhs)
    {
        if (rhs == null)
            return false;
        
        return Handle.Handle == rhs.Handle.Handle;
    }

    public static bool operator ==(UnmanagedObject lhs, UnmanagedObject rhs)
    {
        // Check for null on left side.
        if (ReferenceEquals(lhs, null))
        {
            if (ReferenceEquals(rhs, null))
            {
                // null == null = true.
                return true;
            }

            // Only the left side is null.
            return false;
        }

        // Equals handles case of null on right side.
        return lhs.Equals(rhs);
    }

    public static bool operator !=(UnmanagedObject lhs, UnmanagedObject rhs)
    {
        return !(lhs == rhs);
    }

    public override int GetHashCode()
    {
        return Handle.Handle.ToInt32(); //Handle.GetHashCode();
    }

    public override string ToString()
    {
        return String.Format("{0} 0x{1}", this.GetType().Name, Handle.Handle.ToInt64());
    }
}
