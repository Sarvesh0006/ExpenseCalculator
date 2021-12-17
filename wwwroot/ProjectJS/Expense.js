
$('body').on('click', '#btn_add', function () {
    $(this).attr("disabled");
    
    let Title = $('#txt_title').val();
    let Amount = $('#txt_amount').val();
    let Date = $('#txt_date').val();
    if (Title == "") {
        alert("Enter Title..");
        $('#txt_title').focus();
        $(this).removeAttr('disabled');
        return false;
    }
    if (Amount == "") {
        alert("Enter Amount..");
        $('#txt_amount').focus();
        $(this).removeAttr('disabled');
        return false;
    }
    if (Date == "") {
        alert("Enter Date..");
        $('#txt_date').focus();
        $(this).removeAttr('disabled');
        return false;
    }

    let obj = new Object();
    obj.Title = Title.toString();
    obj.Amount = Amount.toString();
    obj.Date = Date.toString();
    if(confirm('Are you shure ?')) {
        $.ajax({
            url: '/home/InsertExpence',
            type: 'post',
            data: JSON.stringify(obj),
            dataType: 'json',
            contentType: 'application/json charset=UTF-8',
            success: function (data) {
                debugger
                $(this).removeAttr('disabled');
                if (data.responseCode == 200) {
                    alert('Successfully Done..');
                    $('#myModal').modal('hide');
                }
                else {
                    alert(data.responseMessage);
                }
            },
            error: function (xhr) {
                console.log(xhr);
            }
        });
    }
});


Apex = {
	chart: {
		parentHeightOffset: 0,
		toolbar: {
			show: !1
		}
	},
	grid: {
		padding: {
			left: 0,
			right: 0
		}
	},
	colors: ["#5369f8", "#43d39e", "#f77e53", "#1ce1ac", "#25c2e3", "#ffbe0b"],
	tooltip: {
		theme: "dark",
		x: {
			show: !1
		}
	},
	dataLabels: {
		enabled: !1
	},
	xaxis: {
		axisBorder: {
			color: "#d6ddea"
		},
		axisTicks: {
			color: "#d6ddea"
		}
	},
	yaxis: {
		labels: {
			offsetX: -10
		}
	}
},
	function (e) {
		"use strict";

		function t() { }
		t.prototype.initCharts = function () {
			var e = {
				chart: {
					height: 380,
					type: "line",
					zoom: {
						enabled: !1
					},
					toolbar: {
						show: !1
					}
				},
				dataLabels: {
					enabled: !0
				},
				stroke: {
					width: [3, 3],
					curve: "smooth"
				},
				series: [{
					name: "High - 2018",
					data: [28, 29, 33, 36, 32, 32, 33, 22, 11, 5, 55]
				}, {
					name: "Low - 2018",
					data: [12, 11, 14, 18, 17, 13, 13, 2, 3, 4, 5, 9]
				}],
				grid: {
					row: {
						colors: ["transparent", "transparent"],
						opacity: .2
					},
					borderColor: "#e9ecef"
				},
				markers: {
					style: "inverted",
					size: 6
				},
				xaxis: {
					categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
					title: {
						text: "Month"
					},
					axisBorder: {
						color: "#d6ddea"
					},
					axisTicks: {
						color: "#d6ddea"
					}
				},
				yaxis: {
					title: {
						text: "Temperature"
					},
					min: 1,
					max: 60
				},
				legend: {
					position: "top",
					horizontalAlign: "right",
					floating: !0,
					offsetY: -25,
					offsetX: -5
				},
				responsive: [{
					breakpoint: 600,
					options: {
						chart: {
							toolbar: {
								show: !1
							}
						},
						legend: {
							show: !1
						}
					}
				}]
			};
			new ApexCharts(document.querySelector("#apex-line-1"), e).render();
			e = {
				chart: {
					height: 380,
					type: "bar",
					toolbar: {
						show: !1
					}
				},
				plotOptions: {
					bar: {
						dataLabels: {
							position: "top"
						}
					}
				},
				dataLabels: {
					enabled: !0,
					formatter: function (e) {
						return e + "%"
					},
					offsetY: -30,
					style: {
						fontSize: "12px",
						colors: ["#304758"]
					}
				},
				series: [{
					name: "Inflation",
					data: [2.3, 3.1, 4, 10.1, 4, 3.6, 3.2, 2.3, 1.4, .8, .5, .2]
				}],
				xaxis: {
					categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
					position: "top",
					labels: {
						offsetY: -18
					},
					axisBorder: {
						show: !1
					},
					axisTicks: {
						show: !1
					},
					crosshairs: {
						fill: {
							type: "gradient",
							gradient: {
								colorFrom: "#D8E3F0",
								colorTo: "#BED1E6",
								stops: [0, 100],
								opacityFrom: .4,
								opacityTo: .5
							}
						}
					},
					tooltip: {
						enabled: !0,
						offsetY: -35
					}
				},
				fill: {
					gradient: {
						enabled: !1,
						shade: "light",
						type: "horizontal",
						shadeIntensity: .25,
						gradientToColors: void 0,
						inverseColors: !0,
						opacityFrom: 1,
						opacityTo: 1,
						stops: [50, 0, 100, 100]
					}
				},
				yaxis: {
					axisBorder: {
						show: !1
					},
					axisTicks: {
						show: !1
					},
					labels: {
						show: !1,
						formatter: function (e) {
							return e + "%"
						}
					}
				},
				title: {
					text: "Monthly Inflation in Argentina, 2002",
					floating: !0,
					offsetY: 350,
					align: "center",
					style: {
						color: "#444"
					}
				},
				grid: {
					row: {
						colors: ["transparent", "transparent"],
						opacity: .2
					},
					borderColor: "#f1f3fa"
				}
			};
			new ApexCharts(document.querySelector("#apex-column-2"), e).render();
		}, t.prototype.init = function () {
			this.initCharts()
		}, e.ApexChartPage = new t, e.ApexChartPage.Constructor = t
	}(window.jQuery),
	function () {
		"use strict";
		window.jQuery.ApexChartPage.init()
	}();