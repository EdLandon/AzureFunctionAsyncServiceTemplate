using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessLib
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
	public class FuncDocAttribute : Attribute
	{
		public FuncDocAttribute(string domain, string service, string evnt, string version, Type eventContract, string operation) {
			this.Domain = domain;
			this.Service = service;
			this.Evnt = evnt;
			this.Version = version;
			this.EventContract = eventContract;
			this.Operation= operation;
		}

		public string Domain { get; set; }
		public string Service { get; set; }
		public string Evnt { get; set; }
		public string Version { get; set; }
		public Type EventContract{ get; set; }
		public string Operation{ get; set; }
	}
}
