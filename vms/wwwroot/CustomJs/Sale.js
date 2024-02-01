
var SalesOrder = {
    init: function () { },
    count: 1,
    unitAmount: 0,
    totalPrice: 0,
    totalVat: 0,
    SalesDetailList: [],
    ContentInfoJson: [],
    SalesPaymentReceiveJson: [],
    item: [],
    isChecked: 0,
    UnitPrice: 0,
    calculateTotal: function () {
        var vatPercent = $('#VAT').val();
        var Amount = $('#Amount').val();
        var vatValue = parseFloat(Amount);
        var vat = parseFloat(vatPercent) / 100;
        vatValue = vatValue * (vat);
        var total = parseFloat(Amount) + vatValue;
        $('#vatValue').text(vatValue.toFixed(4));
        $('#totalValue').text(total.toFixed(4));
    },
    ForeignCustomerLoad: function () {
        $.ajax({
            type: "GET",
            url: getForeignCust,
            //data: { value: isVDS },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#CustomerIdExport").empty();
                $.each(data,
                    function () {
                        $("#CustomerIdExport").append($("<option/>").val(this.id).text(this.name));
                    });
            },
            failure: function () {
                alert("Failed!");
            }
        });
    },
    FullExportOriented: function () {
        $.ajax({
            type: "GET",
            url: getFullExportOriented,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#CustomerIdExport").empty();
                $.each(data,
                    function () {
                        $("#CustomerIdExport").append($("<option/>").val(this.id).text(this.name));
                    });
            },
            failure: function () {
                alert("Failed!");
            }
        });
    },
    getPurchaseOrder: function () {
        var purchaseOrder = new Object();
        var OrganizationId = $('#OrganizationId').val();
        var VendorId = $('#VendorId').val();
        var Amount = $('#PurchaseAmount').val();
        var DeliveryDate = $('#DeliveryDate').val();
        var Iteams = $('#PurchaseIteams').val();
        var Vat = $('#PurchaseVat').val();
        purchaseOrder.OrganizationId = OrganizationId;
        purchaseOrder.VendorId = VendorId;
        purchaseOrder.Amount = Amount;
        purchaseOrder.ExtDeliveryDate = ExtDeliveryDate;
        purchaseOrder.DeliveryDate = DeliveryDate;
        purchaseOrder.Iteams = Iteams;
        purchaseOrder.Vat = Vat;

    },
    CalculateTotalPriceAndVat: function (totalPrice, totalVat) {
        SalesOrder.totalPrice += totalPrice;
        SalesOrder.totalVat += totalVat;
        $('#TotalPrice').text(SalesOrder.totalPrice);
        $('#TotalVat').text(SalesOrder.totalVat);
    },
    gridTableEmpty: function () {
        $('#ProductId').val('');
        $('#Product').val('');
        $('#Amount').val('');
        $('#item').val('');
        $('#amount').val('');
        $('#VAT').val('');
        $('#MaxSaleQty').val('');
        $('#SupplementaryDutyPercent').val('');
        $('#InitialAmount').val('');
        $('#VatTypeId').val(0);
        $('#unit').val('');
        $('#vatValue').text('');
        $('#totalValue').text('');
        SalesOrder.UnitPrice = 0;
    },
    gridPaymentEmpty: function () {
        $('#PaymentMethodId').val('');
        $('#PaidAmount').val('0');
        $('#PaymentDate').val('');
        $('#ChequeNumber').val('');
        $('.chequeSelection').hide();
    },
    gridContentEmpty: function () {
        $('#DocumentTypeId').val('');
        $('#FileUpload').val('');
    },
    addDocument: function () {
        var DocumentName = $('#DocumentTypeId option:selected').text();
        var DocumentTypeId = $('#DocumentTypeId').val();
        var files = $("#FileUpload").get(0).files;
        var table = document.getElementById('contentTable');
        var rowCount = table.rows.length - 1;
        if (DocumentTypeId > 0) {
            var canAdd = true;

            if (canAdd) {
                var data = new Object();
                data.rowCount = "789" + rowCount;
                data.DocumentTypeId = DocumentTypeId;
                data.UploadFile = files[0];
                SalesOrder.ContentInfoJson.push(data);

                var html = '<tr id="' +
                    data.rowCount +
                    '">' +

                    '<td>' +
                    DocumentName +
                    '</td>' +
                    '<td>' +
                    files[0].name +
                    '</td>' +
                    '<td><span onclick="DeleteDocument(' +
                    data.rowCount +
                    ')"  class="glyphicon glyphicon-minus btn-xs"></a></span></td>';

                html += '</tr>';

                $("table#contentTable > tbody").append(html);
                SalesOrder.gridContentEmpty();
            }
        }

    },
    isNumber: function (n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    },
    addPayment: function () {

        var PaymentMethod = $('#PaymentMethodId option:selected').text();
        var PaymentMethodId = $('#PaymentMethodId').val();
        var PaidAmount = $("#PaidAmount").val();
        var PaymentDate = $("#PaymentDate").val();
        var table = document.getElementById('payment');
        var ChequeNumber = $("#ChequeNumber").val();

        var rowCount = table.rows.length - 1;
        if (PaymentMethodId > 0) {
            var canAdd = true;

            if (canAdd) {
                var data = new Object();
                //PurchaseDetails
                data.rowCount = "987" + rowCount;
                data.PaymentMethodId = PaymentMethodId;
                data.PaidAmount = PaidAmount;
                data.PaymentDate = PaymentDate;
                data.ChequeNumber = ChequeNumber;
                SalesOrder.SalesPaymentReceiveJson.push(data);

                if (PaymentMethodId == 2) {

                    var html = '<tr id="' +
                        data.rowCount +
                        '">' +
                        '<td>' +
                        rowCount +
                        '</td>' +
                        '<td>' +
                        PaymentMethod +
                        '</td>' +
                        '<td>' +
                        ChequeNumber +
                        '</td>' +
                        '<td>' +
                        PaidAmount +
                        '</td>' +
                        '<td>' +
                        PaymentDate +
                        '</td>' +
                        '<td><span onclick="DeletePayment(' +
                        data.rowCount +
                        ')"  class="glyphicon glyphicon-minus btn-xs"></a></span></td>';

                    html += '</tr>';

                    $("table#payment > tbody").append(html);
                } else {

                    var html = '<tr id="' +
                        data.rowCount +
                        '">' +
                        '<td>' +
                        rowCount +
                        '</td>' +
                        '<td>' +
                        PaymentMethod +
                        '</td>' +
                        '<td>' +

                        '</td>' +
                        '<td>' +
                        PaidAmount +
                        '</td>' +
                        '<td>' +
                        PaymentDate +
                        '</td>' +
                        '<td><span onclick="DeletePayment(' +
                        data.rowCount +
                        ')"  class="glyphicon glyphicon-minus btn-xs"></a></span></td>';
                    html += '</tr>';
                    $("table#payment > tbody").append(html);
                }

                SalesOrder.gridPaymentEmpty();

            }
        }

    },
    SetPaidAmount: function (amount) {

        var sum = 0;
        var currentSum = 0;
        if (SalesOrder.SalesDetailList.length == 0) {
            $('#PaidAmount').val('0');
        } else {
            $.each(SalesOrder.SalesDetailList,
                function (i, v) {
                    var vatValue = SalesOrder.SalesDetailList[i].VATPercent / 100;
                    var totalVat = (SalesOrder.SalesDetailList[i].Quantity *
                        SalesOrder.SalesDetailList[i].UnitPrice) *
                        vatValue;
                    sum += totalVat + (SalesOrder.SalesDetailList[i].Quantity * SalesOrder.SalesDetailList[i].UnitPrice);

                });
            if (SalesOrder.SalesPaymentReceiveJson.length > 0) {
                $.each(SalesOrder.SalesPaymentReceiveJson,
                    function (i, v) {

                        currentSum += SalesOrder.SalesPaymentReceiveJson[i].ReceiveAmount;

                    });
            }
            var finalAmount = sum - currentSum;
            $('#PaidAmount').val(finalAmount.toFixed(4));

        }
    },
    add: function () {
        var productName = $('#Product').val();
        var vatTypeValue = $('#VatTypeId option:selected').text();
        var productId = $('#ProductId').val();
        var vendor = $('#VendorId option:selected').text();
        var vendorId = $('#VendorId').val();
        var PurchaseInvoice = $('#PurchaseInvoice').val();
        var item = $('#item').val();
        var InitialAmount = $('#InitialAmount').val();
        var IsVatDeductedInSource = $('#IsVatDeductedInSource').is(":checked");
        var IsExport = $('#IsExport').is(":checked");
        var amount = $('#amount').val();
        var vatType = $('#VatTypeId').val();
        var vatPercent = $('#VAT').val();
        var SupplementaryDutyPercent = $('#SupplementaryDutyPercent').val();
        if (IsExport) {
            vatType = 1;
            vatPercent = 0;
            SupplementaryDutyPercent = 0;
        }
        var unit = $('#unit option:selected').text();
        var unitId = $('#unit').val();
        var Amount = $('#Amount').val();
        var MaxSaleQty = $('#MaxSaleQty').val();
        var VDSAmount = $('#VDSAmount').val();
        if (productId > 0) {

            var canAdd = true;

            $.each(SalesOrder.SalesDetailList,
                function (i, v) {
                    if (SalesOrder.SalesDetailList[i].ProductId == productId) {
                        alert('Sorry! product Order Exists.');
                        canAdd = false;

                    }

                });

            if (canAdd) {

                if (!SalesOrder.isNumber(vatPercent)) {
                    return alert("Input VAT %");
                }

                // var vat =
                var vat = parseFloat(vatPercent) / 100;
                var data = new Object();
                //Sale Details Table
                data.ProductId = productId
                data.ProductVattypeId = vatType;
                data.Quantity = item;
                data.UnitPrice = amount;

                data.DiscountPerItem = 0;
                data.MeasurementUnitId = unitId;
                data.VATPercent = vatPercent;
                data.SupplementaryDutyPercent = SupplementaryDutyPercent;
                data.UnitAmount = amount;
                //TODO
                var vatValue = parseFloat(Amount);
                vatValue = (vatValue * (vat));
                var total = parseFloat(Amount) + vatValue;
                data.totalPrice = total;
                data.totalVat = vatValue;

                SalesOrder.SalesDetailList.push(data);
                SalesOrder.SetPaidAmount(total);
                if (IsVatDeductedInSource) {
                    var previousAmount = parseFloat(VDSAmount);
                    $('#VDSAmount').val((vatValue + previousAmount).toFixed(4))
                }
                if (IsExport) {
                    var html = '<tr id="' +
                        data.ProductId +
                        '">' +
                        '<td>' +
                        SalesOrder.count +
                        '</td>' +
                        '<td>' +
                        productName +
                        '</td>' +
                        '<td>' +
                        MaxSaleQty +
                        '</td>' +
                        '<td>' +
                        item +
                        '</td>' +
                        '<td>' +
                        Amount +
                        '</td>' +
                        '<td>' +
                        amount +
                        '</td>' +
                        '<td>' +
                        unit +
                        '</td>' +
                        '<td><span onclick="Delete(' +
                        data.ProductId +
                        ')"  class="btn-xs glyphicon glyphicon-minus"></a></span></td>';

                    html += '</tr>';

                    $("table#gridTable > tbody").append(html);
                    SalesOrder.CalculateTotalPriceAndVat(total.toFixed(4), vatValue.toFixed(4));
                    SalesOrder.gridTableEmpty();
                    SalesOrder.count++;
                } else {
                    var html = '<tr id="' +
                        data.ProductId +
                        '">' +
                        '<td>' +
                        SalesOrder.count +
                        '</td>' +
                        '<td>' +
                        productName +
                        '</td>' +
                        '<td>' +
                        MaxSaleQty +
                        '</td>' +
                        '<td>' +
                        item +
                        '</td>' +
                        '<td>' +
                        Amount +
                        '</td>' +
                        '<td>' +
                        SupplementaryDutyPercent +
                        '</td>' +
                        '<td>' +
                        amount +
                        '</td>' +
                        '<td>' +
                        unit +
                        '</td>' +
                        '<td>' +
                        vatPercent +
                        '</td>' +
                        '<td>' +
                        vatTypeValue +
                        '</td>' +
                        '<td>' +
                        vatValue.toFixed(4) +
                        '</td>' +
                        '<td>' +
                        total.toFixed(4) +
                        '</td>' +
                        '<td><span onclick="Delete(' +
                        data.ProductId +
                        ')"  class="btn-xs glyphicon glyphicon-minus"></a></span></td>';

                    html += '</tr>';

                    $("table#gridTable > tbody").append(html);
                    SalesOrder.CalculateTotalPriceAndVat(total.toFixed(4), vatValue.toFixed(4));
                    SalesOrder.gridTableEmpty();
                    SalesOrder.count++;
                }
            }
        }

    },

    delete: function (id) {
        var IsVatDeductedInSource = $('#IsVatDeductedInSource').is(":checked");

        for (var key in SalesOrder.SalesDetailList) {
            var value = SalesOrder.SalesDetailList[key].ProductId;
            if (SalesOrder.SalesDetailList[key].ProductId == id) {
                SalesOrder.count -= 1;
                SalesOrder.CalculateTotalPriceAndVat(-SalesOrder.SalesDetailList[key].totalPrice,
                    -SalesOrder.SalesDetailList[key].totalVat);
                //SalesOrder.SetPaidAmount(-SalesOrder.SalesDetailList[key].totalPrice);
                if (IsVatDeductedInSource) {
                    var previousVDSAmount = $('#VDSAmount').val();
                    $('#VDSAmount').val(parseFloat(previousVDSAmount) -
                        SalesOrder.SalesDetailList[key].totalVat.toFixed(4));
                }

            }
        }

        $.each(SalesOrder.SalesDetailList,
            function (i, v) {
                if (SalesOrder.SalesDetailList[i].ProductId == id) {
                    SalesOrder.SalesDetailList.splice(i, 1);
                    if (SalesOrder.SalesPaymentReceiveJson.length > 0) {
                        PaymentInfoReset();

                    }
                    SalesOrder.SetPaidAmount(0);
                }
                $("tr#" + id).remove().fadeOut();

            });

    },
    deleteDocument: function (id) {

        $.each(SalesOrder.ContentInfoJson,
            function (i, v) {
                if (SalesOrder.ContentInfoJson[i].rowCount == id) {

                    SalesOrder.ContentInfoJson.splice(i, 1);

                }
                $(" tr#" + id).remove().fadeOut();

            });

    },
    deletePayment: function (id) {

        $.each(SalesOrder.SalesPaymentReceiveJson,
            function (i, v) {
                if (SalesOrder.SalesPaymentReceiveJson[i].rowCountPayment == id) {

                    SalesOrder.SalesPaymentReceiveJson.splice(i, 1);

                }
                $("tr#" + id).remove().fadeOut();

            });

    },
    ProductVatForExport: function (isExport) {
        //alert(isImports);
        $.ajax({
            type: "GET",
            url: productVatType,
            data: { value: isExport },

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //$("#VatTypeId").clear();
                $("#VatTypeId").empty();
                $.each(data,
                    function () {
                        $("#VatTypeId").append($("<option/>").val(this.id).text(this.name));
                    });
            },
            failure: function () {
                alert("Failed!");
            }
        });
    },
    ProductVatForLocal: function (isVDS) {
        $.ajax({
            type: "GET",
            url: productTypeSaleVds,
            data: { value: isVDS },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#VatTypeId").empty();
                $.each(data,
                    function () {
                        $("#VatTypeId").append($("<option/>").val(this.id).text(this.name));
                    });
            },
            failure: function () {
                alert("Failed!");
            }
        });
    },
    calculate: function (value) {

        var unitAmount = $('#amount').val();

        if (!SalesOrder.isNumber(unitAmount) || !(SalesOrder.isNumber(value))) {

            return;
        } else {
            $('#Amount').val((unitAmount * value).toFixed(4));
            SalesOrder.calculateTotal();
        }

    },
    calculateUnitPrice: function (value) {
        //debugger
        var qty = $('#item').val();
        if (!SalesOrder.isNumber(qty) || !(SalesOrder.isNumber(value))) {

            return;
        } else {
            $("#amount").val((value / qty).toFixed(4));
            SalesOrder.calculateTotal();
        }

    },
    calculateAmount: function (value) {
        //debugger
        var qty = $('#item').val();
        if (!SalesOrder.isNumber(qty) || !(SalesOrder.isNumber(value))) {

            return;
        } else {
            $("#Amount").val((value * qty).toFixed(4));
            SalesOrder.calculateTotal();
        }

    },
    Save: function () {
        //Get Data for Sale
        var IsExport = $('#IsExport').is(":checked");
        var ReceiverName = $('#ReceiverName').val();
        var ReceiverContactNo = $('#ReceiverContactNo').val();
        var CustomerId = $('#CustomerId').val();
        var ShippingAddress = $('#ShippingAddress').val();
        var ShippingCountryId = $('#ShippingCountryId').val();
        var SalesTypeId = 1;
        var IsVatDeductedInSource = $('#IsVatDeductedInSource').is(":checked");
        if (IsExport) {
            SalesTypeId = 2;
            IsVatDeductedInSource = false;

            CustomerId = $('#CustomerIdExport').val();
        }
        var ExportTypeId = $('#ExportTypeId').val();

        var DeliveryDate = $('#DeliveryDate').val();

        var LcNo = $('#LcNo').val();
        var LcDate = $('#LcDate').val();
        var BillOfEntry = $('#BillOfEntry').val();
        var BillOfEntryDate = $('#BillOfEntryDate').val();
        var DueDate = $('#DueDate').val();
        var TermsOfLc = $('#TermsOfLc').val();
        var CustomerPoNumber = $('#CustomerPoNumber').val();
        var SalesDeliveryTypeId = $('#SalesDeliveryTypeId').val();
        var VDSAmount = $('#VDSAmount').val();
        var VehicleRegNo = $('#VehicleRegNo').val();
        var VehicleType = $('#VehicleType').val();
        var data = new FormData();

        for (var i = 0; i < SalesOrder.SalesDetailList.length; i++) {
            data.append('SalesDetailList[' + i + '].ProductId', SalesOrder.SalesDetailList[i].ProductId);
            data.append('SalesDetailList[' + i + '].ProductVattypeId',
                SalesOrder.SalesDetailList[i].ProductVattypeId);
            data.append('SalesDetailList[' + i + '].Quantity', SalesOrder.SalesDetailList[i].Quantity);
            data.append('SalesDetailList[' + i + '].UnitPrice', SalesOrder.SalesDetailList[i].UnitAmount);
            data.append('SalesDetailList[' + i + '].DiscountPerItem',
                SalesOrder.SalesDetailList[i].DiscountPerItem);
            data.append('SalesDetailList[' + i + '].SupplementaryDutyPercent',
                SalesOrder.SalesDetailList[i].SupplementaryDutyPercent);
            data.append('SalesDetailList[' + i + '].VATPercent', SalesOrder.SalesDetailList[i].VATPercent);
            data.append('SalesDetailList[' + i + '].MeasurementUnitId',
                SalesOrder.SalesDetailList[i].MeasurementUnitId);
        }
        //TODO
        for (var i = 0; i < SalesOrder.ContentInfoJson.length; i++) {
            data.append('ContentInfoJson[' + i + '].DocumentTypeId',
                SalesOrder.ContentInfoJson[i].DocumentTypeId);
            data.append('ContentInfoJson[' + i + '].UploadFile', SalesOrder.ContentInfoJson[i].UploadFile);

        }
        //TODO
        for (var i = 0; i < SalesOrder.SalesPaymentReceiveJson.length; i++) {
            data.append('SalesPaymentReceiveJson[' + i + '].ReceivedPaymentMethodId',
                SalesOrder.SalesPaymentReceiveJson[i].PaymentMethodId);
            data.append('SalesPaymentReceiveJson[' + i + '].ReceiveAmount',
                SalesOrder.SalesPaymentReceiveJson[i].PaidAmount);
            data.append('SalesPaymentReceiveJson[' + i + '].ChequeNumber',
                SalesOrder.SalesPaymentReceiveJson[i].ChequeNumber);
            data.append('SalesPaymentReceiveJson[' + i + '].ReceiveDate',
                SalesOrder.SalesPaymentReceiveJson[i].PaymentDate);
        }

        data.append("OrganizationId", 1);
        data.append("DiscountOnTotalPrice", 0);
        data.append("IsVatDeductedInSource", IsVatDeductedInSource);
        data.append("CustomerId", CustomerId);
        data.append("ReceiverName", ReceiverName);
        data.append("ReceiverContactNo", ReceiverContactNo);
        data.append("ShippingAddress", ShippingAddress);
        data.append("ShippingCountryId", ShippingCountryId);
        data.append("SalesTypeId", SalesTypeId); //TODO
        data.append("SalesDeliveryTypeId", SalesDeliveryTypeId); //TODO
        data.append("SalesDate", '2019-01-01'); //TODO
        data.append("DeliveryDate", DeliveryDate); //TODO
        data.append("VehicleRegNo", VehicleRegNo); //TODO
        data.append("VehicleTypeId", VehicleType); //TODO

        if (IsExport) {
            data.append("ExportTypeId", ExportTypeId); //TODO
            data.append("LcNo", LcNo); //TODO
            data.append("LcDate", LcDate); //TODO
            data.append("BillOfEntry", BillOfEntry); //TODO
            data.append("BillOfEntryDate", BillOfEntryDate); //TODO
            data.append("DueDate", DueDate); //TODO
            data.append("TermsOfLc", TermsOfLc); //TODO
        }
        data.append("VDSAmount", VDSAmount);
        data.append("CustomerPoNumber", CustomerPoNumber); //TODO
        data.append("IsTaxInvoicePrined", 0); //TODO
        data.append("CreatedBy", 1); //TODO
        data.append("CreatedTime", '2019-01-01'); //TODO
        data.append("__RequestVerificationToken", $("input[name=__RequestVerificationToken]").val());

        $.ajax({
            url: save,
            data: data,
            type: "POST",
            processData: false,
            contentType: false,
            dataType: "json",
            beforeSend: function () {
                $("#loading").show();
            },

            success: function (result) {
                $("#showData").html("");

                if (result == false) {
                    alert("Please add atleast on Sale Details");
                } else {
                    window.location.href = returnUrl;
                }
            },
            error: function (x, e) {
                $("#loading").hide();
                alert('error');
            }
        });
    },
    product: {
        init: function () { },
        productAutoComplete: function () {
            var materialforrequsition = {
                Initialize: function () {
                    materialforrequsition.Typehead();
                },

                Typehead: function () {
                    $('#Product').typeahead('destroy');
                    $('#Product').typeahead({
                        hint: true,
                        highlight: true,
                        minLength: 2,
                    },
                        {
                            items: 8,
                            name: 'Product',
                            displayKey: function (s) {
                                return s.name;
                            },
                            property: "ProductId",
                            source: function (strmaterial, process) {
                                var url = prodDetailsAutoComplt;
                                return $.getJSON(url,
                                    { filterText: $("#Product").val() },
                                    function (Data) {

                                        return process(Data);
                                    });
                            },
                            updater: function (item) {
                                return item.name;
                            }
                        }).on('typeahead:selected',
                            function (obj, datum) {
                                $("#ProductId").val(datum.id);
                                $('#Amount').val(datum.unitPrice);
                                $('#item').val(1);
                                $('#unit').val(datum.unit);
                                $('#amount').val(datum.unitPrice);
                                $('#VAT').val(datum.vat);
                                $('#MaxSaleQty').val(datum.maxSaleQty);
                                $('#VatTypeId').val(datum.productVATTypeId);
                                $('#SupplementaryDutyPercent').val(datum.supplimentaryDuty);
                                SalesOrder.unitAmount = datum.unitPrice;
                                SalesOrder.UnitPrice = datum.unitPrice;
                                SalesOrder.calculateTotal();
                                HideAddButton(datum.maxSaleQty);
                                VatTypeChange();
                                SevenPointFivePercent();
                            });
                    $("#Product").focus();

                }
            }
            materialforrequsition.Initialize();
        }
    }
}

$(document).ready(function () {

    $('#salesForeign').validate({
        errorClass: 'help-block animation-slideDown',
        errorElement: 'div',
        errorPlacement: function (error, e) {
            e.parents('.form-group >div').append(error);
        },
        highlight: function (e) {
            $(e).closest('.form-group').removeClass('has-success').addClass('has-error');
            $(e).closest('.help-block').remove();
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error').addClass('has-success');

        },
        success: function (e) {
            $(e).closest('.form-group').removeClass('has-success has-error')

        },
        rules: {
            CustomerId: {
                valueNotEquals: "0"
            },
            CustomerIdExport: {
                valueNotEquals: "0"
            },
            CustomerContactNo: {
                number: true,
                minlength: 11,
                maxlength: 11
            },
            SalesDeliveryTypeId: {
                valueNotEquals: "null"
            },
            DeliveryDate: {
                date: true,
            },
            //Discount: {
            //    required: true,
            //    number: true
            //},
            ReceiverName: {
                required: function (element) {
                    if ($("#CustomerId").val() == "0") {
                        return true;
                    } else {
                        return false;
                    }
                },
                maxlength: 200
            },
            ReceiverContactNo: {
                required: function (element) {
                    if ($("#CustomerId").val() == "0") {
                        return true;
                    } else {
                        return false;
                    }
                },
                number: true,
                minlength: 11,
                maxlength: 11
            },
            ShippingAddress: {
                required: function (element) {
                    if ($("#CustomerId").val() == "0") {
                        return true;
                    } else {
                        return false;
                    }
                },
                maxlength: 200
            },

            item: {
                required: true,
                greaterThan: true,
                number: true
            },
            amount: {
                required: $("#amount").val() < 0,
                maxlength:8,
                number: true
            },
            Product: {
                required: true
            },
            DiscountPerItem: {
                number: true
            },
            SupplementaryDutyPercent: {
                number: true
            },
            LcNo: {
                maxlength: 50
            },
            LcDate: {
                date: true,
            },
            BillOfEntry: {
                maxlength: 50
            },
            BillOfEntryDate: {
                date: true,
            },
            DueDate: {
                date: true,
            },
            CustomerPoNumber: {
                maxlength: 50
            },
            PaidAmount: {
                number: true,
                maxlength:8
            },
            PaymentDate: {
                required: true,
                date: true
            }
        },
        messages: {
            CustomerId: {
                valueNotEquals: 'Select Customer'
            },
            CustomerIdExport: {
                valueNotEquals: 'Select a Customer'
            },
            CustomerContactNo: {
                number: 'Enter Mobile Number',
                minlength: 'Number need to be 11 digit',
                maxlength: 'Number need to be 11 digit'
            },
            SalesDeliveryTypeId: {
                valueNotEquals: "Select a DeliveryType"
            },
            DeliveryDate: {
                date: 'Provide a valid Date',
            },
            ReceiverName: {
                required: 'Please provide ReceiverName',
                maxlength: 'ReceiverName Can not more than 200 char'
            },
            ReceiverContactNo: {
                required: 'Enter Number',
                number: 'Enter Mobile Number',
                minLength: 'Number need to be 11 digit',
                maxlength: 'Number need to be 11 digit'
            },
            Discount: {
                required: 'Please provide Discount',
                number: 'Entrer Number'
            },
            ShippingAddress: {
                required: 'Enter Address',
                maxlength: 'ShippingAddress Can not more than 200 char'
            },
            VatChallanNo: {
                maxlength: 'VatChallanNo Can not more than 50 char'
            },
            item: {
                required: 'Please provide Quantity',
                number: 'Entrer Number'
            },
            amount: {
                required: 'Please provide amount',
                maxlength:'Please add a valid number',
                number: 'Entrer Number'
            },
            Product: {
                required: 'Please Enter Product',
            },
            DiscountPerItem: {
                number: 'Entrer Number'
            },
            SupplementaryDutyPercent: {
                number: 'Entrer Number'
            },
            LcNo: {
                maxlength: 'Value Can not more than 50 char'
            },
            LcDate: {
                date: 'Provide a valid Date',
            },
            BillOfEntry: {
                maxlength: 'Value Can not more than 50 char'
            },
            BillOfEntryDate: {
                date: 'Provide a valid Date'
            },
            DueDate: {
                date: 'Provide a valid Date'
            },
            CustomerPoNumber: {
                maxlength: 'Value Can not more than 50 char'
            },
            PaidAmount: {
                number: 'Entrer Number',
                maxlength:'Please add a valid number'
            },
            PaymentDate: {
                required: 'Provide Payment date',
                date: 'Provide a valid Date'
            }

        }
    });
    $.validator.addMethod("greaterThan",
        function (value, element, param) {
            var val_a = parseInt($("#MaxSaleQty").val());
            return this.optional(element) || (parseInt(value) != 0 && parseInt(value) <= val_a);
        },
        "Please Check Quantity");
    $.validator.addMethod("valueNotEquals",
        function (value, element, arg) {
            return arg !== value;
        },
        "Please Select value");

    $('#add').click(function () {
        $("#Product").rules('add', 'required');
        $("#item").rules('add', 'required');
        $("#amount").rules('add', 'required');
        $("#PaymentDate").rules('remove', 'required');
        $("#SalesDeliveryTypeId").rules('remove', 'valueNotEquals');
        $("#CustomerId").rules('remove', 'valueNotEquals');
        $("#CustomerIdExport").rules('remove', 'valueNotEquals');
        $("#Discount").rules('remove', 'required');
        $("#ReceiverName").rules('remove', 'required');
        $("#ReceiverContactNo").rules('remove', 'required');
        $("#ShippingAddress").rules('remove', 'required');
        if ($("#salesForeign").valid()) {

            SalesOrder.add();
            return true;
        } else {
            return false;
        }

    });

    $('#save').click(function () {
        $("#Product").rules('remove', 'required');
        $("#item").rules('remove', 'required');
        $("#amount").rules('remove', 'required');
        $("#PaymentDate").rules('remove', 'required');
        $("#Discount").rules('add', 'required');
        $("#SalesDeliveryTypeId").rules('add',
            {
                valueNotEquals: 'null'
            });
        if ($('#IsExport:checked').length == 1) {

            $("#CustomerId").rules('remove', 'required');
            $("#CustomerIdExport").rules('add',
                {
                    valueNotEquals: '0'


                });

            $("#ShippingAddress").rules('add',
                {
                    required: function () {
                        if ($("#CustomerIdExport").val() == "0") {
                            return true;
                        } else {
                            return false;
                        }
                    }
                });
            $("#ReceiverContactNo").rules('add',
                {
                    required: function () {
                        if ($("#CustomerIdExport").val() == "0") {
                            return true;
                        } else {
                            return false;
                        }
                    }
                });
            $("#ReceiverName").rules('add',
                {
                    required: function () {
                        if ($("#CustomerIdExport").val() == "0") {
                            return true;
                        } else {
                            return false;
                        }
                    }
                });
        } else {
            $("#CustomerIdExport").rules('remove', 'required');
            $("#CustomerId").rules('add',
                {
                    valueNotEquals: '0'


                });
            $("#ReceiverContactNo").rules('add',
                {
                    required: function () {
                        if ($("#CustomerId").val() == "0") {
                            return true;
                        } else {
                            return false;
                        }
                    }
                });
            $("#ShippingAddress").rules('add',
                {
                    required: function () {
                        if ($("#CustomerId").val() == "0") {
                            return true;
                        } else {
                            return false;
                        }
                    }
                });
            $("#ReceiverName").rules('add',
                {
                    required: function () {
                        if ($("#CustomerId").val() == "0") {
                            return true;
                        } else {
                            return false;
                        }
                    }
                });
        }

        if ($("#salesForeign").valid()) {
            var rowPurchase = $('#tblPurchaseOD tr').length;

            if (rowPurchase > 0) {
                if (confirm('Are you sure?')) {
                    SalesOrder.Save();
                    return true;
                }
            } else {
                alert('Add Product');
            }
        } else {
            alert('Please Enter Required Fields!');
            return false;
        }

    });
    //$("#ExpectedDeleveryDate").datepicker();
    //$("#ExpectedDeleveryDate").datepicker('option', 'dateFormat', 'dd M, yy');
    //$("#PaymentDate").datepicker();
    //$("#PaymentDate").datepicker('option', 'dateFormat', 'dd M, yy');
    //$("#DeliveryDate").datepicker();
    //$("#DeliveryDate").datepicker('option', 'dateFormat', 'dd M, yy');
    //$("#LcDate").datepicker();
    //$("#LcDate").datepicker('option', 'dateFormat', 'dd M, yy');
    //$("#BillOfEntryDate").datepicker();
    //$("#BillOfEntryDate").datepicker('option', 'dateFormat', 'dd M, yy');
    //$("#DueDate").datepicker();
    //$("#DueDate").datepicker('option', 'dateFormat', 'dd M, yy');
    SalesOrder.product.productAutoComplete();
});

function DeleteDocument(id) {
    SalesOrder.deleteDocument(id);
};

function DeletePayment(id) {
    SalesOrder.deletePayment(id);
};

function Delete(id) {
    SalesOrder.delete(id);
};

function GridInfoReset() {
    $("#tblPurchaseOD").empty();
    $("#paymentdt").empty();
    $("#contentTabledt").empty();
    $('#salesForeign')[0].reset();
    SalesOrder.SalesDetailList = [];
    SalesOrder.ContentInfoJson = [];
    SalesOrder.SalesPaymentReceiveJson = [];
    SalesOrder.UnitPrice = 0;
    $("#vatValue").text('');
    $("#totalValue").text('');
    //$("#ExpectedDeleveryDate").datepicker();
    //$("#ExpectedDeleveryDate").datepicker('option', 'dateFormat', 'dd M, yy');
    //$("#PaymentDate").datepicker();
    //$("#PaymentDate").datepicker('option', 'dateFormat', 'dd M, yy');
    //$("#DeliveryDate").datepicker();
    //$("#DeliveryDate").datepicker('option', 'dateFormat', 'dd M, yy');
    //$("#LcDate").datepicker();
    //$("#LcDate").datepicker('option', 'dateFormat', 'dd M, yy');
    //$("#BillOfEntryDate").datepicker();
    //$("#BillOfEntryDate").datepicker('option', 'dateFormat', 'dd M, yy');
    //$("#DueDate").datepicker();
    //$("#DueDate").datepicker('option', 'dateFormat', 'dd M, yy');
};
function PaymentInfoReset() {
    $("#paymentdt").empty();
    SalesOrder.SalesPaymentReceiveJson = [];
}
$("#item").keyup(function (evt) {

    var that = this,
        numberofitem = $(this).val();
    //var regex = new regexp("^023-[0-9]{0,7}$");
    //if (regex.test(numberofitem)) {
    //    return evt.preventdefault();

    //}


    SalesOrder.calculate(numberofitem);
    SevenPointFivePercent();

});
$('#addDocument').click(function () {
    SalesOrder.addDocument();
});
$('#addPayment').click(function () {
    $("#Product").rules('remove', 'required');
    $("#item").rules('remove', 'required');
    $("#amount").rules('remove', 'required');
    $("#PaymentDate").rules('add', 'required');

    $("#ReceiverName").rules('remove', 'required');
    $("#ReceiverContactNo").rules('remove', 'required');
    $("#ShippingAddress").rules('remove', 'required');
    $("#Discount").rules('remove', 'required');
    if ($("#salesForeign").valid()) {
        SalesOrder.addPayment();
        return true
    } else {
        return false;
    }

});
$("#Amount").keyup(function () {
    var that = this,
        amount = $(this).val();
    SalesOrder.calculateUnitPrice(amount);
    SevenPointFivePercent();

});
$("#amount").keyup(function () {
    var that = this,
        amount = $(this).val();
    SalesOrder.calculateAmount(amount);
    SevenPointFivePercent();
});
$("#VAT").keyup(function () {

    SalesOrder.calculateTotal();

});
$("#CustomerId").change(function () {
    //            debugger
    var end = this.value;
    var id = $('#CustomerId').val();
    $.ajax({
        url: getLocalCust,
        dataType: "json",
        type: "GET",
        data: { id: id },
        beforeSend: function () {
            $("#loading").show();
        },
        success: function (result) {
            $("#loading").hide();

            if (result == false) {
                alert("Please add atleast on purchase Details");
            } else {
                $('#CustomerContactNo').val(result.phoneNo);
                $('#ReceiverName').val(result.name);
                $('#ReceiverContactNo').val(result.phoneNo);
                $('#ShippingAddress').val(result.address);
                // alert(result);
            }
        },
        error: function (x, e) {
            $("#loading").hide();
            alert("Error");
        }
    });
});
$("#CustomerIdExport").change(function () {
    //            debugger
    var end = this.value;
    var id = $('#CustomerIdExport').val();
    $.ajax({
        url: getLocalCust,
        dataType: "json",
        type: "GET",
        data: { id: id },
        beforeSend: function () {
            $("#loading").show();
        },
        success: function (result) {
            $("#loading").hide();

            if (result == false) {
                alert("Something Wrong!");
            }
            else {
                $('#CustomerContactNo').val(result.phoneNo);
                $('#ReceiverName').val(result.name);
                $('#ReceiverContactNo').val(result.phoneNo);
                $('#ShippingAddress').val(result.address);
                $('#ShippingCountryId').val(result.countryId);
                // alert(result);
            }
        },
        error: function (x, e) {
            $("#loading").hide();
            alert("Error");
        }
    });
});
$("#PaymentMethodId").change(function () {

    var id = $("#PaymentMethodId").val();
    if (id == 2) {
        $('.chequeSelection').show();
    } else {
        $('.chequeSelection').hide();
    }

});
$('#IsExport').change(function () {
    //            debugger
    if ($('#IsExport:checked').length == 0) {
        SalesOrder.ProductVatForExport(false);
        SalesOrder.isChecked = $("#IsExport:checked").length;
        GridInfoReset();
        $('.p_element').hide();
        $('.vds_element').show();
        $('.details_element').show();
        $("#IsExport").attr("checked", false);
    } else {
        SalesOrder.ProductVatForExport(true);
        SalesOrder.isChecked = $("#IsExport:checked").length;
        //alert(checked);
        $('.p_element').show();
        $('.vds_element').hide();
        $('.details_element').hide();
        GridInfoReset();
        $("#IsExport").attr("checked", SalesOrder.isChecked);
    }

});
$('#IsVatDeductedInSource').change(function () {
    // debugger
    if ($('#IsVatDeductedInSource:checked').length == 0) {

        SalesOrder.ProductVatForLocal(false);
        GridInfoReset();
        $("#IsVatDeductedInSource").attr("checked", false);

    } else {
        //purchasOrder.isVDS = $("#IsVatDeductedInSource:checked").length;
        SalesOrder.ProductVatForLocal(true);
        GridInfoReset();
        $("#IsVatDeductedInSource").attr("checked", true);

    }

});
$('#ExportTypeId').change(function () {
    var id = $('#ExportTypeId').val();
    if (id == 1) {
        SalesOrder.ForeignCustomerLoad();

    } else if (id == 2) {
        SalesOrder.FullExportOriented();
    } else {
        SalesOrder.ForeignCustomerLoad();
    }

});
$('#VatTypeId').change(function () {

    VatTypeChange();

});
function HideAddButton(qty) {
    var x = document.getElementById("add");
    if (qty == 0) {
        x.setAttribute('class', 'hidden');
    } else {
        x.setAttribute('class', 'visible');
    }

}
function SevenPointFivePercent() {
    var x = document.getElementById("add");
    var div = document.getElementById('sevenPointFiveDiv');
    var percent = 7.5;
    var percentValue = (percent / 100)
    var valueAbove = SalesOrder.UnitPrice + (SalesOrder.UnitPrice * percentValue);
    var valueDown = SalesOrder.UnitPrice - (SalesOrder.UnitPrice * percentValue);
    var currentUnitPrice = $('#amount').val();
    if (currentUnitPrice > valueAbove) {
        $('#alertMessage').text('Over 7.5');
        div.setAttribute('class', 'alert alert-danger visible');
        x.setAttribute('class', 'visible');
        return true;
    }
    else if (currentUnitPrice < valueDown) {
        $('#alertMessage').text('Less than 7.5! You Can not Sale');
        div.setAttribute('class', 'alert alert-danger visible');
        x.setAttribute('class', 'hidden');
        return false;
    } else {
        div.setAttribute('class', 'alert alert-danger hidden');
        x.setAttribute('class', 'visible');
        return true;
    }
}
function VatTypeChange() {
    var id = $('#VatTypeId').val();
    if (id == 3) {
        $('#VAT').val('15');
        SalesOrder.calculateTotal();
        $('#VAT').attr('readonly', 'true');

    } else if (id == 2) {
        $('#VAT').val('0');
        SalesOrder.calculateTotal();
        $('#VAT').attr('readonly', 'true');

    } else {
        $('#VAT').attr('readonly', false);

    }
}

