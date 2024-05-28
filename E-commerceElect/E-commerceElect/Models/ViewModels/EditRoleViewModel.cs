﻿using System.ComponentModel.DataAnnotations;

namespace E_commerceElect.Models.ViewModels
{
	public class EditRoleViewModel
	{
		public EditRoleViewModel()
		{
			Users = new List<string>();
		}

		public string Id { get; set; }
		[Required ]
		public string RoleName { get; set; }
		public List<string> Users { get; set; }
	}
}
