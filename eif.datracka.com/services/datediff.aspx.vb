
Partial Class services_datediff
    Inherits System.Web.UI.Page
    'Declare the dates.


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load




        Label1.Text = CType(TiempoDiff("09/09/2009 16:00"), String)

    End Sub

    Public Function TiempoDiff(ByVal dTimeOn As Date) As Integer

        Dim dTimeOff As Date = Now()
        Dim intTotalTimeWorked As Integer

        Dim strTimeon As String
        Dim strTimeoff As String


        'Format the dates.
        strTimeon = dTimeOn.ToString("hh:mm tt")
        strTimeoff = dTimeOff.ToString("hh:mm tt")
        intTotalTimeWorked = DateDiff(DateInterval.Minute, dTimeOn, dTimeOff)
        Return intTotalTimeWorked
    End Function

    Public Function TimeDiff(ByVal t1, ByVal t2)

        'Purpose: Calculate the difference between two given times
        ' t1 and t2. Returns t1 - t2.

        Dim sec1, sec2, secOut, minsOut, hoursOut

        'Convert times to seconds
        sec1 = (Hour(t1) * 3600) + (Minute(t1) * 60) + Second(t1)
        sec2 = (Hour(t2) * 3600) + (Minute(t2) * 60) + Second(t2)

        'Subtract one from the other
        secOut = sec1 - sec2 ' or Add together to sum two times

        'Calc hours
        hoursOut = Int(secOut / 3600)
        secOut = secOut - (hoursOut * 3600)

        'Calc minutes
        minsOut = Int(secOut / 60)
        secOut = secOut - (minsOut * 60)

        'Convert back to HH:MM:SS
        TimeDiff = TimeValue(Format(hoursOut, "00") + ":" + Format(minsOut, "00") + ":" + Format(secOut, "00"))

    End Function

End Class
