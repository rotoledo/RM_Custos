using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml;
using Xunit;


namespace RM_Custos
{
	public class UnitTest1
	{

		public string Envelope = @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:br='http://www.totvs.com.br/br/'>
			   <soapenv:Header/>
			   <soapenv:Body>
				  <br:ReadRecordAuth>
					 <br:DataServerName>GchPacienteData</br:DataServerName>
					 <br:PrimaryKey>1;1;100630</br:PrimaryKey>
					 <br:Contexto>CODCOLIGADA=1;CODSISTEMA=0;CODUSUARIO=mestre</br:Contexto>
					 <br:Usuario>mestre</br:Usuario>
			         <br:Senha>totvs</br:Senha>
				  </br:ReadRecordAuth>
			   </soapenv:Body>
			</soapenv:Envelope>";

		[Fact]
		public async void Test01()
		{
			// GIVEN
			StringContent content = new StringContent(Envelope, Encoding.UTF8, "text/xml");
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://10.51.5.196/totvsbusinessconnect/wsDataServer.asmx");
			request.Headers.Add("soapaction", "http://www.totvs.com.br/br/ReadRecordAuth");
			request.Content = content;

			// WHEN
			HttpClient client = new HttpClient();
			HttpResponseMessage response = await client.SendAsync(request);
			string result = response.Content.ReadAsStringAsync().Result;
			


			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(result);
			xmlDocument.GetElementsByTagName("ReadRecordAuthResult");
			XmlNodeList list = xmlDocument.GetElementsByTagName("ReadRecordAuthResult");
			XmlNode node = list.Item(0);
			string ReadRecordAuthResult = node.InnerText;

			// THEN
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Contains("CLARICE PINTO", result);
		}


		//Outra forma de fazer
		//request.Method = HttpMethod.Post;
		//request.RequestUri = new Uri("http://10.51.5.196/totvsbusinessconnect/wsDataServer.asmx");
	}
}


