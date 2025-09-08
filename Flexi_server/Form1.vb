Imports System.Data.SqlClient
Imports System.IO.Ports
Imports System.Management
Imports System.Threading
Imports System.Text.RegularExpressions

Public Class Form1
    Dim revdata As String = ""
    Dim b As Integer
    Dim con As New SqlConnection
    Dim cmd As New SqlCommand
    Dim rd As SqlDataReader
    Dim loopbreak As Integer


    Public Event DataReceived As IO.Ports.SerialDataReceivedEventHandler

    Dim WithEvents Serialport1 As New IO.Ports.SerialPort

    
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        con.ConnectionString = "Server=localhost;Database=Flexiload;User Id=sa;Password=123;"
        cmd.Connection = con
        Serialport1.PortName = TextBox1.Text
        Serialport1.BaudRate = 9600
        Serialport1.Parity = IO.Ports.Parity.None
        Serialport1.DataBits = 8
        Serialport1.StopBits = IO.Ports.StopBits.One
        Serialport1.Handshake = IO.Ports.Handshake.None
        'Serialport1.RtsEnable = True

        Serialport1.Open()
        Timer1.Start()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim flagid As Int32
        Dim Phone As String
        Dim Amount As Int32
        Dim rid As Int32

        
        con.Open()

        cmd.CommandText = "SELECT * FROM [Flexiload].[dbo].[Pending] Where Status=0"
        rd = cmd.ExecuteReader
        rd.Read()
        If rd.HasRows Then
            'rd.NextResult()
            'abc = rd.GetString(0)
            flagid = rd.GetInt32(3)
            Phone = rd.GetString(1)
            Amount = rd.GetInt32(2)
            rid = rd.GetInt32(0)
            con.Close()

            If flagid = 0 Then
                Serialport1.Write("AT+STGI=0" & vbCrLf)
                Threading.Thread.Sleep(300)
                Serialport1.Write("AT+STGR=0,1,128" & vbCrLf)
                Threading.Thread.Sleep(1500)
                Serialport1.Write("AT+STGI=6" & vbCrLf)
                Threading.Thread.Sleep(300)
                Serialport1.Write("AT+STGR=6,1,1" & vbCrLf)
                Threading.Thread.Sleep(1500)
                Serialport1.Write("AT+STGI=3" & vbCrLf)
                Threading.Thread.Sleep(300)
                Serialport1.Write("AT+STGR=3,1" & vbCr)
                Threading.Thread.Sleep(300)
                Serialport1.Write(Phone & Chr(26)) 'Cnumber
                Threading.Thread.Sleep(1500)
                Serialport1.Write("AT+STGI=3" & vbCrLf)
                Threading.Thread.Sleep(300)
                Serialport1.Write("AT+STGR=3,1" & vbCr)
                Threading.Thread.Sleep(300)
                Serialport1.Write(Phone & Chr(26)) 'Cnumber2
                Threading.Thread.Sleep(1500)
                Serialport1.Write("AT+STGI=3" & vbCrLf)
                Threading.Thread.Sleep(1500)
                Serialport1.Write("AT+STGR=3,1" & vbCr)
                Threading.Thread.Sleep(300)
                Serialport1.Write(Amount & Chr(26))  'amount
                Threading.Thread.Sleep(1500)
                Serialport1.Write("AT+STGI=3" & vbCrLf)
                Threading.Thread.Sleep(1500)
                Serialport1.Write("AT+STGR=3,1" & vbCr)     'amount2
                Threading.Thread.Sleep(300)
                Serialport1.Write(Amount & Chr(26))
                Threading.Thread.Sleep(1500)
                Serialport1.Write("AT+STGI=3" & vbCrLf)
                Threading.Thread.Sleep(1500)
                Serialport1.Write("AT+STGR=3,1" & vbCr)
                Threading.Thread.Sleep(300)
                Serialport1.Write("2580" & Chr(26) & vbCrLf) 'pin
                Threading.Thread.Sleep(1500)
                Serialport1.Write("AT+STGI=1" & vbCrLf)
                Threading.Thread.Sleep(1500)
                Serialport1.Write("AT+STGR=1,1" & vbCrLf)
                Threading.Thread.Sleep(1500)
                Serialport1.Write("AT+STGI=9" & vbCrLf)
                Threading.Thread.Sleep(1500)
                'loopbreak = 1
                

                con.Open()

                cmd.CommandText = "UPDATE  [Flexiload].[dbo].[Pending] SET Status=1 WHere ID=" & rid
                rd = cmd.ExecuteReader
                con.Close()

                Label8.Text = ("FlexiLoaD Done")
                
            End If
        Else
            con.Close()

        End If

    End Sub

    Private Sub Serialport1_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles Serialport1.DataReceived
        Dim datain As String = ""
        Dim numbytes As Integer = Serialport1.BytesToRead
        For i As Integer = 1 To numbytes
            datain &= Chr(Serialport1.ReadChar)
        Next
        test(datain)

    End Sub

    Private Sub test(ByVal indata As String)
        revdata &= indata
        'revdata.Substring(revdata.Length, )
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        '  If revdata.ToString.Contains("+CMTI") Then
        revdata = ""
        Serialport1.Write("AT+CMGF=1" & vbCrLf)
        Threading.Thread.Sleep(100)

        'Serialport1.Write("AT+CPMS=""ME""" & vbCrLf)
        'Threading.Thread.Sleep(100)

        Serialport1.Write("AT+CMGL=""ALL""" & vbCrLf)
        Threading.Thread.Sleep(4000)
        ReadMsg()
        'End If
    End Sub

    Private Sub ReadMsg()
        Dim fmns As Integer

        Dim lineoftext As String
        Dim i As Integer
        Dim arytextfile() As String

        lineoftext = revdata.ToString
        'revdata = ""
        arytextfile = Split(lineoftext, "+CMGL", , CompareMethod.Text)
        Dim phonen As String
        Dim messageb As String
        Dim datetimee As String
        Dim messagebody As String
        Dim mphone As String
        Dim mindex As Integer



        For i = 2 To UBound(arytextfile)
            Dim input As String = arytextfile(i)
            Dim result() As String
            Dim pattern As String = "(: )|(,"")|("","")|("",,"")|("","")|("",:"")|("" "")"
            result = Regex.Split(input, pattern)
            phonen = result(6)
            messageb = result(8)
            datetimee = Mid(messageb, 1, 17)
            mindex = result(2)
            Serialport1.Write("AT+CMGD=" & mindex & vbCrLf)
            Threading.Thread.Sleep(100)
            Dim sss As Integer


            messagebody = Mid(messageb, 22, messageb.Length - 22)
            sss = InStr(1, messagebody, vbCrLf + vbCrLf)
            If (sss > 0) Then
                messagebody = Mid(messagebody, 1, sss)
                Dim a As String

                con.Open()
                a = "INSERT INTO Flexiload.dbo.Receive_sms VALUES (" & "'" & phonen & "'" & "," & "'" & messagebody & "'" & "," & "'" & DateTime.Now().ToString("yyyy-MM-dd HH:mm:ss") & "'" & ")"
                cmd.CommandText = a
                rd = cmd.ExecuteReader
                con.Close()

            End If
            Dim success As Integer
            Dim tid As Integer
            Dim tid1 As String
            Dim sorry As Integer


            Label7.Text = messagebody
            ' mphone = Mid(messagebody, 2, TextBox2.Text.Length)
            fmns = InStr(1, messagebody, " 17")
            mphone = Mid(messagebody, fmns + 1, 10)
            success = InStr(1, messagebody, "successful")
            sorry = InStr(1, messagebody, "Sorry")
            tid = InStr(1, messagebody, "transaction ID ")
            tid1 = Mid(messagebody, tid + 14, 18)
            If fmns > 0 And success > 0 Then
                con.Open()
                '     'INSERT INTO [AkashSMS].dbo.SendSMS VALUES ('1','+171154021','hello','successs','2014-01-01 9:00:00')
                Dim stest As String
                Dim flagid As Int32
                Dim Phone As String
                Dim Amount As Int32
                Dim rid As Integer
                Dim rf As String

                Dim dt As String = DateTime.Now().ToString("yyyy-MM-dd HH:mm:ss")



                stest = "SELECT * FROM [Flexiload].[dbo].[Pending] where Status=1 and Phone_Number LIKE " + "'" + "%" + mphone + "%" + "'"
                cmd.CommandText = stest
                rd = cmd.ExecuteReader
                rd.Read()

                If rd.HasRows Then


                    'rd.NextResult()
                    'abc = rd.GetString(0)
                    flagid = rd.GetInt32(3)
                    Phone = rd.GetString(1)
                    Amount = rd.GetInt32(2)
                    rf = rd.GetString(4)
                    rid = rd.GetInt32(0)
                    con.Close()
                    con.Open()

                    cmd.CommandText = "DELETE FROM [Flexiload].[dbo].[Pending] where ID=" & rid
                    rd = cmd.ExecuteReader
                    con.Close()
                    con.Open()

                    cmd.CommandText = "INSERT INTO [Flexiload].[dbo].[Successfull] (Phone_Number,Amount,Status,Request_From,Date_Time,Transaction_ID,FlagId) VALUES ('" & Phone & "','" & Amount & "','" & "SUCCESSFULL" & "','" & rf & "','" & dt & "','" & tid1 & "','" & flagid & "')"

                    rd = cmd.ExecuteReader
                    con.Close()
                
                End If
            End If
            If fmns > 0 And sorry > 0 Then
                Dim stest As String
                Dim flagid As Int32
                Dim Phone As String
                Dim Amount As Int32
                Dim rid As Integer
                Dim rf As String

                Dim dt As String = DateTime.Now().ToString("yyyy-MM-dd HH:mm:ss")



                con.Open()
                stest = "SELECT * FROM [Flexiload].[dbo].[Pending] where Status=1 and Phone_Number LIKE " + "'" + "%" + mphone + "%" + "'"
                cmd.CommandText = stest
                rd = cmd.ExecuteReader
                rd.Read()
                If rd.HasRows Then
                    rid = rd.GetInt32(0)
                    flagid = rd.GetInt32(3)
                    Phone = rd.GetString(1)
                    Amount = rd.GetInt32(2)
                    rf = rd.GetString(4)

                    con.Close()
                    con.Open()
                    cmd.CommandText = "DELETE FROM [Flexiload].[dbo].[Pending] where ID=" & rid
                    rd = cmd.ExecuteReader
                    con.Close()
                    con.Open()

                    cmd.CommandText = "INSERT INTO [Flexiload].[dbo].[Successfull] (Phone_Number,Amount,Status,Request_From,Date_Time,Transaction_ID,FlagId) VALUES ('" & Phone & "','" & Amount & "','" & "REJECTED" & "','" & rf & "','" & dt & "','" & "NOT AVAILABLE" & "','" & flagid & "')"

                    rd = cmd.ExecuteReader
                    con.Close()
                End If
            Else
                con.Close()


            End If
            b = b + 1
            Label6.Text = Str(b)


        Next
        '   Serialport1.Write("AT+CMGD=1,1" & vbCrLf)
        '         Threading.Thread.Sleep(100)


    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        Button2.PerformClick()
        Threading.Thread.Sleep(200)
        Button3.PerformClick()
        Threading.Thread.Sleep(200)


        Timer1.Start()

    End Sub

End Class
