module PERTish.Charts

// http://msdn.microsoft.com/en-us/library/vstudio/hh297122(v=vs.100).aspx

open System
open System.Drawing
open System.Windows.Forms
open System.Windows.Forms.DataVisualization.Charting

let makeForm title =
    let form = new Form(Visible = true, 
                        Width = 500, Height = 300)
    form.Text <- title
    form.FormClosed.Add(fun _ -> Application.Exit())
    form

let addControl (form:Form) control =
    form.Controls.Add(control)

let makeChart (s:(float * float) seq) =
    let chart = new Chart(Dock = DockStyle.Fill)
    chart.ChartAreas.Add(new ChartArea("MainArea"))
    let series = new Series(ChartType = SeriesChartType.Area)
    chart.Series.Add(series)
    for x,y in s do
        series.Points.AddXY(x, y) |> ignore
    chart

let showChart title data =
    let form = makeForm title
    let chart = makeChart data
    addControl form chart
