# Emrys.API 
1、获取参数
1.1、所有参数均在Request里，Request可以获取封装以后所有的接收数据
  string cmd = Reqeust.cmd;
  int pageNo = Reqeust.pageNo;
  object paramses = Reqeust.Params;

1.2、在Params可以获取所有Params里所有的数据 
  string userName = Convert.ToString(Params["UserName"]);

1.3、可以通过方法获取数据
  string userName2 = GetParams<string>("UserName");
  string pwd = GetParams<string>("Pwd");
  int age = GetParams<int>("Age");

1.4、通过GetParams的第二个参数，可以设置参数是否是必须传入的参数，默认为必须传入的参数
  string userName3 = GetParams<string>("UserName", false); // UserName不是必须需要传入的参数
  string userName4 = GetParams<string>("UserName", true); // UserName是必须需要传入的参数

1.5、时间格式做了统一的处理 传入格式为long类型的时间戳 如：1466871403000
  DateTime time = GetParams<DateTime>("Time");

1.6、如传入的参数有很多，通过方法GetParams一个一个获取则比较麻烦，所以如果遇到参数较多的情况下，需要新建一个Class，属性与Params参数一致即可,如LoginRequestModel，通过方法ConvertToModel则可把参数全部封装到类中，方便使用参数。
  LoginRequestModel login = ConvertToModel<LoginRequestModel>();

1.7、可以在传入参数Class中上和类的属性上标记特性[APIRequired]来标记参数是否是必须的参数。

2、设置返回值
2.1、 直接设置返回值
  return APIJson(new { name = "emrys" });

2.2、 返回String
  return APIContext("xxxxxxxxxxxxxx");

2.3、返回对象
  return APIJson(new LoginRequestModel { });

2.4 直接返回需要的对象值 
  return new APIResultJson { resultNote = "xxxxxxx" };
