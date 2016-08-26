package md57a5e2202be1f4c89cbd45492b2eba6fb;


public class PickWeek
	extends android.app.DialogFragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MaydSchedulerApp.PickWeek, MaydScheduler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", PickWeek.class, __md_methods);
	}


	public PickWeek () throws java.lang.Throwable
	{
		super ();
		if (getClass () == PickWeek.class)
			mono.android.TypeManager.Activate ("MaydSchedulerApp.PickWeek, MaydScheduler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
