using Model.DataBaseModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Model;
using Model.XmlModel;

namespace AsyncServer
{
    public class ClientProcess
    {
        private readonly UserContext _db;
        private int? _peopleId;

        public ClientProcess(UserContext db)
        {
            _db = db;
        }

        public async void Process(TcpClient tcpClient)
        {
            var clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();
            Debug.WriteLine("Received connection request from " + clientEndPoint);
            try
            {
                var networkStream = tcpClient.GetStream();
                var reader = new StreamReader(networkStream);
                var writer = new StreamWriter(networkStream) {AutoFlush = true};
                while (true)
                {
                    var s = await reader.ReadLineAsync();
                    if (s == null) break;

                    Debug.WriteLine("Received service request: " + s);
                    var result = Response(s);

                    var str = Converters.ObjectToXml(result);
                    Debug.WriteLine("Computed response is: \n");
                    await writer.WriteLineAsync(str);

                    if (result.Code == 2) break;
                }
                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                if (tcpClient.Connected)
                    tcpClient.Close();
            }
        }

        private Result Response(string request)
        {
            var requestObject = Converters.XmlToObject<Request>(request);
            var result = new Result {Code = 1};
            if (requestObject == null)
                return result;
            if (_peopleId == null && requestObject.Function != Functions.Authorization) return new Result {Code = 2};

            switch (requestObject.Function)
            {
                case Functions.GetGoodsList:
                    result = GoodsList();
                    break;
                case Functions.GetDescription:
                    break;
                case Functions.SendOrder:
                    break;
                case Functions.GetOrderConfirm:
                    break;
                case Functions.SetState:
                    break;
                case Functions.ConfirmOrder:
                    break;
                case Functions.GetOrderList:
                    result = OrderList(requestObject);
                    break;
                case Functions.Authorization:
                    result = Authorization(requestObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        #region service methods

        private Result Authorization(Request obj)
        {
            var login = obj.Attribute
                .SingleOrDefault(r => r.Name == "login")
                ?.Value;
            var password = obj.Attribute
                .SingleOrDefault(r => r.Name == "password")
                ?.Value;

            var people = _db.Peoples
                .SingleOrDefault(x
                    => x.Name == login && x.Password == password);
            _peopleId = people?.Id ?? -1;
            return people != null
                ? new Result {Code = 0}
                : new Result {Code = 2};
        }

        private Result GoodsList()
        {
            _db.Goods.Load();
            var goodsList = _db.Goods.Local.ToList();
            return new Result
            {
                Code = 0,
                Data = new Data
                {
                    Nested = new Nested
                    {
                        Data = goodsList.Select(x => new Data
                        {
                            Input = new List<Input>
                            {
                                new Input
                                {
                                    Key = nameof(x.Id),
                                    Value = x.Id.ToString()
                                },
                                new Input
                                {
                                    Key = nameof(x.Name),
                                    Value = x.Name
                                },
                                new Input
                                {
                                    Key = nameof(x.Price),
                                    Value = x.Price.ToString(CultureInfo.InvariantCulture)
                                },
                                new Input
                                {
                                    Key = nameof(x.Description),
                                    Value = x.Description
                                }
                            }
                        }).ToList()
                    }
                }
            };
        }

        private Result OrderList(Request obj)
        {
            var people = _db.Peoples.Single(x => x.Id == _peopleId);
            var ordersByPeople = people.Orders;
            var orders = ordersByPeople.GroupBy(x => x.OrderNumber).Select(o => new Order()
            {
                OrderNumber = o.Key,
                OrderDate = o.First().OrderDate,
                State = o.First().State,
            }).ToList();
            return new Result()
            {
                Code = 0,
                Data = new Data()
                {
                    Nested = new Nested()
                    {
                        Data = orders.Select(x => new Data()
                        {
                            Input = new List<Input>()
                            {
                                new Input
                                {
                                    Key = nameof(x.OrderNumber),
                                    Value = x.OrderNumber.ToString()
                                },
                                new Input
                                {
                                    Key = nameof(x.OrderNumber),
                                    Value = x.OrderDate.ToString(CultureInfo.InvariantCulture)
                                },
                                new Input
                                {
                                    Key = nameof(x.OrderNumber),
                                    Value = x.State.ToString()
                                }
                            }
                        }).ToList()
                    }
                }
            };
        }

        #endregion
    }
}