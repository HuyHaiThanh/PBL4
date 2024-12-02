using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpFileDownloader
{

    public class ClaimURL
    {
        public List<string> downloadLinks = new List<string>();
        public void ClaimExtension()
        {
            HttpListener listener = new HttpListener();
            // Địa chỉ lắng nghe

            listener.Prefixes.Add("http://localhost:5000/catch/");
            // Xử lý yêu cầu trong vòng lặp
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {

                        listener.Start();
                    // Chờ yêu cầu từ extension
                    var context = listener.GetContext();

                        // Lấy dữ liệu từ request
                        using (var reader = new StreamReader(context.Request.InputStream, Encoding.UTF8))
                        {
                            string requestData = reader.ReadToEnd();
                            Console.WriteLine("Received data: " + requestData);

                            // Parse link từ JSON gửi tới
                            string downloadLink = ExtractLinkFromJson(requestData);

                            if (!string.IsNullOrEmpty(downloadLink) && !downloadLinks.Contains(downloadLink))
                            {
                                // Thêm vào danh sách link tải xuống
                                downloadLinks.Add(downloadLink);
                                Console.WriteLine($"Download link saved: {downloadLink}");
                                Console.WriteLine(Observer.Instance != null);
                                Observer.Instance.Broadcast(EventId.OnGetLinkByHttp, downloadLink);
                            }
                        }

                        // Gửi phản hồi lại cho extension
                        context.Response.StatusCode = 200; // HTTP 200 OK
                        byte[] responseBytes = Encoding.UTF8.GetBytes("Link received successfully!");
                        context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                        context.Response.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            });

            listener.Stop();
        }
        public static void Main(string[] args)
        {
            

        }

        // Phương thức phân tích JSON và lấy link tải xuống
        public string ExtractLinkFromJson(string json)
        {
            try
            {
                // Dữ liệu JSON từ extension có dạng: {"url":"http://example.com/file.zip"}
                var start = json.IndexOf("\"url\":\"") + 7;
                var end = json.IndexOf("\"", start);
                if (start > 0 && end > start)
                {
                    return json.Substring(start, end - start);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing JSON: " + ex.Message);
            }
            return null;
        }
    }
}
