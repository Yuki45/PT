using System;

namespace WindowsApplication3
{
	/// <summary>
	/// 
	/// </summary>
	public class cfeature_vector
	{
		public ctrack_node[] tracks;
		public cfeature_vector()
		{
			tracks=new ctrack_node[6] ;
			for (int i=0;i<6;i++)
			{
				tracks[i]=new ctrack_node() ;
			}
		}
		public void feature_extractor(cpixel_node []pixels,int size)
		{
			for(int i=0;i<size;i++)
			{
				
				tracks[pixels[i].track].sectors[pixels[i].sector].relations[pixels[i].relation]++ ;
			
			}
		}
	}
}
