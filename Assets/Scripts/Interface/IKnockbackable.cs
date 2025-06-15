using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
	public interface IKnockbackable
	{
		/// <summary>
		/// Đẩy enemy theo một hướng với lực cho trước.
		/// </summary>
		/// <param name="direction">Hướng từ attacker tới enemy.</param>
		/// <param name="force">Lực đẩy (impulse).</param>
		void KnockBack(Vector2 direction, float force);
	}
}
