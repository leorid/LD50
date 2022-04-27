using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace JL
{
	public class MessageHubTestScriptEditor
	{
		bool received;
		void GetNr(int nr)
		{
			received = true;
		}
		void GetMsg()
		{
			received = true;
		}

		// A Test behaves as an ordinary method
		[Test]
		public void HubGlobalSubscribeAndExecuteNoParams()
		{
			// ARRANGE
			received = false;

			// ACT
			EventHub.Global.Subscribe(this, "GetNr", GetMsg);
			EventHub.Global.Execute(this, "GetNr");
			EventHub.Global.UnSubscribe(this, "GetNr", GetMsg);

			// ASSERT
			Assert.True(received);
		}

		[Test]
		public void HubGlobalUnsubscribeTestNoParams()
		{
			// ARRANGE
			received = false;

			// ACT
			EventHub.Global.Subscribe(this, "GetNr", GetMsg);
			EventHub.Global.UnSubscribe(this, "GetNr", GetMsg);
			EventHub.Global.Execute(this, "GetNr");

			// ASSERT
			Assert.False(received);
		}

		[Test]
		public void HubGlobalSubscribeAndExecuteOneParam()
		{
			// ARRANGE
			received = false;

			// ACT
			EventHub.Global.Subscribe<int>(this, "GetNr", GetNr);
			EventHub.Global.Execute<int>(this, "GetNr", 9);
			EventHub.Global.UnSubscribe<int>(this, "GetNr", GetNr);

			// ASSERT
			Assert.True(received);
		}

		[Test]
		public void HubGlobalUnsubscribeTestOneParam()
		{
			// ARRANGE
			received = false;

			// ACT
			EventHub.Global.Subscribe<int>(this, "GetNr", GetNr);
			EventHub.Global.UnSubscribe<int>(this, "GetNr", GetNr);
			EventHub.Global.Execute<int>(this, "GetNr", 9);

			// ASSERT
			Assert.False(received);
		}
	}
}
