using Autofac;

namespace Server.WebAPI.AutofacIOC
{
    public class IOCNameModuleRegister : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            AutofacModuleRegister autofacModuleRegister = new AutofacModuleRegister();
            autofacModuleRegister.Create(builder);
        }
    }
}
