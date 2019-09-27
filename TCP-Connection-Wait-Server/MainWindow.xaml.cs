using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TCP_Connection_Wait_Server
{
    /// <summary>
    /// TCL 상태를 알려준다
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker backgroundWorker;
        public MainWindow()
        {
            InitializeComponent();
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerAsync();
        }

        TcpListener listener;
        TcpClient client;
        NetworkStream stream;
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            listener = new TcpListener(IPAddress.Any, 9999); // 모든 IP 에서 9999포트로 들어오는 값을 받습니다.
            listener.Start(); // 받기 시작
            client = listener.AcceptTcpClient(); // 클라이언트가 접속을 기다립니다.
            stream = client.GetStream(); // 통신을 위한 stream을 저장합니다.
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(stream is null) { return; } // 클라이언트가 접속하지 않았으면 리턴
            string message = XAML_Message_tb.Text; // 보낼 메세지
            byte[] sendBytes = Encoding.UTF8.GetBytes(message); // 메세지를 UTF8 방식으로 인코딩
            int len = sendBytes.Length; // 보낼 데이터의 길이
            stream.WriteByte((byte) (len / 256 / 256));
            stream.WriteByte((byte)(len / 256));
            stream.WriteByte((byte)(len % 256));
            stream.Write(sendBytes, 0, len); //sendByte를 len 길이 만큼 보냄

            XAML_Message_tb.Text = ""; // 보낸 후 초기화
        }
    }
}
