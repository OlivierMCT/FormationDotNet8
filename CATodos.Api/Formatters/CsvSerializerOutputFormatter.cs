using CATodos.Api.Dtos;
using CATodos.BusinessModels;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;

namespace CATodos.Api.Formatters {
    public class CsvSerializerOutputFormatter : TextOutputFormatter {
        public CsvSerializerOutputFormatter() {
            SupportedMediaTypes.Add("text/csv");
            SupportedEncodings.Add(Encoding.UTF8);
        }

        protected override bool CanWriteType(Type? type) {
            return type is not null && type == typeof(IEnumerable<CategoryDto>);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding) {
            var writer = new StreamWriter(context.HttpContext.Response.Body, Encoding.UTF8);
            //await writer.WriteLineAsync("CSV by CA");
            var props = typeof(CategoryDto).GetProperties();

            await writer.WriteLineAsync(string.Join(",", props.Select(p => p.Name)));
            foreach (CategoryDto obj in (IEnumerable<CategoryDto>) context.Object!) {
                await writer.WriteLineAsync(
                    string.Join(
                        ",", 
                        props.Select(p => p.PropertyType == typeof(string) ? $@"""{p.GetValue(obj)}""" : p.GetValue(obj))
                    )
                );
            }

            await writer.FlushAsync();
        }
    }
}
