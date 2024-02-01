/// <reference path="../js/validation.js" />

var purchasOrder = {
    init: function () { },
    count: 1,
    unitAmount: 0,
    totalPrice: 0,
    totalAmount: 0,
    totalVat: 0,
    limit: 100000,
    typeId: 0,
    purchaseOrders: [],
    PurchaseOrderDetailList: [],
    ContentInfoJson: [],
    PurchasePaymenJson: [],
    item: [],
    isChecked: 0,
    isVDS: 0,
    calculateTotal: function () {

        var vatValue = 0,
            vat = 0,
            total = 0;
        var vatPercent = $('#VAT').val();
        var unitAmount = $('#amount').val();
        var VatTypeId = $('#VatTypeId').val();

        if (VatTypeId == 6) {
            vatValue = parseFloat($('#item').val());
            vat = parseFloat(vatPercent);
            vatValue = vatValue * (vat);
            total = parseFloat(unitAmount) + vatValue;
        } else {
            vatValue = parseFloat(unitAmount);
            vat = parseFloat(vatPercent) / 100;
            vatValue = vatValue * (vat);
            total = parseFloat(unitAmount) + vatValue;
        }
        if (!Number.isNaN(vatValue)) {
            $('#vatValue').text(vatValue.toFixed(4));
        }
        if (!Number.isNaN(total)) {
            $('#totalValue').text(total.toFixed(4));
        }


    },
    UpperLimitPaymentMethod: function (total) {

        purchasOrder.totalAmount += parseFloat(total);
        var isVDS = $('#IsVatDeductedInSource').is(":checked");
        var select = document.getElementById('PaymentMethodId');
        if ((parseFloat(purchasOrder.totalAmount) >= parseFloat(purchasOrder.limit)) && isVDS) {

            for (i = 0; i < select.length; i++) {
                if (select.options[i].value == '1') {
                    select.remove(i);
                }
            }
        } else {
            for (i = 0; i < select.length; i++) {
                if (select.options[i].value == '1') {
                    select.remove(i);
                }
            }
            var option = document.createElement("option");
            option.text = "Cash";
            option.value = 1
            select.add(option);

        }
    },

    purchaseForImport: function (purchaseTypeId) {
        if (purchaseTypeId == 3) {

            $('#PurchaseTypeId').attr("disabled", true);
            $('#gridTable tbody').empty();

            purchasOrder.gridTableEmpty();
            purchasOrder.PurchaseOrderDetailList.length = 0;
            purchasOrder.count = 1;
            purchasOrder.totalPrice = 0;
            purchasOrder.totalVat = 0;
            $('#TotalPrice').text('0');
            $('#TotalVat').text('0');
        }
    },
    purchaseForVDS: function (purchaseTypeId) {
        if (purchaseTypeId == 3) {

            $('#PurchaseTypeId').attr("disabled", true);
            $('#gridTable tbody').empty();

            purchasOrder.gridTableEmpty();
            purchasOrder.PurchaseOrderDetailList.length = 0;
            purchasOrder.count = 1;
            purchasOrder.totalPrice = 0;
            purchasOrder.totalVat = 0;
            $('#TotalPrice').text('0');
            $('#TotalVat').text('0');
        }
    },
    ProductVatForImport: function (isImports) {
        $.ajax({
            type: "GET",
            url: getVDS,
            data: { value: isImports },

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#VatTypeId").empty();
                $.each(data,
                    function () {
                        $("#VatTypeId").append($("<option />").val(this.id).text(this.name));
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
            url: getVDS,
            data: { value: isVDS },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $("#VatTypeId").empty();
                $.each(data,
                    function () {
                        $("#VatTypeId").append($("<option />").val(this.id).text(this.name));
                    });
            },
            failure: function () {
                alert("Failed!");
            }
        });
    },
    gridTableEmpty: function () {
        $('#ProductId').val('');
        $('#Product').val('');
        $('#Amount').val('');
        $('#item').val('');
        $('#SupplementaryDutyPercent').val('');
        $('#DefaultImportDutyPercent').val('');
        $('#DefaultRegulatoryDutyPercent').val('');
        $('#DefaultAdvanceTaxPercent').val('');
        $('#DefaultAdvanceIncomeTaxPercent').val('');
        $('#amount').val('0');
        $('#VAT').val('');
        $('#InitialAmount').val('');
        $('#vatValue').text('');
        $('#totalValue').text('');
        $('#VatTypeId').val('');
        $('#unit').val('');
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

    CalculateTotalPriceAndVat: function (totalPrice, totalVat) {
        purchasOrder.totalPrice += totalPrice;
        purchasOrder.totalVat += totalVat;
        $('#TotalPrice').text(purchasOrder.totalPrice);
        $('#TotalVat').text(purchasOrder.totalVat);
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
                //PurchaseDetails
                data.rowCount = "789" + rowCount;
                data.DocumentTypeId = DocumentTypeId;
                data.UploadFile = files[0];
                purchasOrder.ContentInfoJson.push(data);

                var html = '<tr id="' +
                    data.rowCount +
                    '">' +
                    '<td>' +
                    rowCount +
                    '</td>' +
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
                //purchasOrder.count++;
                purchasOrder.gridContentEmpty();

            }
        }

    },
    isNumber: function (n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    },
    addPayment: function () {
        debugger
        var PaymentMethod = $('#PaymentMethodId option:selected').text();
        var PaymentMethodId = $('#PaymentMethodId').val();
        var PaidAmount = $("#PaidAmount").val();
        var PaymentDate = $("#PaymentDate").val();
        var ChequeNumber = $("#ChequeNumber").val();
        var table = document.getElementById('payment');

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
                purchasOrder.PurchasePaymenJson.push(data);

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

                purchasOrder.gridPaymentEmpty();

            }
        }

    },
    SetPaidAmount: function (amount) {

        var sum = 0;
        var currentSum = 0;
        if (purchasOrder.PurchaseOrderDetailList.length == 0) {
            $('#PaidAmount').val('0');
        } else {
            $.each(purchasOrder.PurchaseOrderDetailList,
                function (i, v) {
                    var vatValue = purchasOrder.PurchaseOrderDetailList[i].VATPercent / 100;
                    var totalVat = (purchasOrder.PurchaseOrderDetailList[i].Quantity *
                        purchasOrder.PurchaseOrderDetailList[i].UnitPrice) *
                        vatValue;
                    sum += totalVat + (purchasOrder.PurchaseOrderDetailList[i].Quantity * purchasOrder.PurchaseOrderDetailList[i].UnitPrice);

                });
            if (purchasOrder.PurchasePaymenJson.length > 0) {
                $.each(purchasOrder.PurchasePaymenJson,
                    function (i, v) {

                        currentSum += purchasOrder.PurchasePaymenJson[i].PaidAmount;

                    });
            }
            var finalAmount = sum - currentSum;
            $('#PaidAmount').val(finalAmount.toFixed(4));

        }

    },
    add: function () {

        var productName = $('#Product').val();
        var productId = $('#ProductId').val();
        var item = $('#item').val();
        var amount = $('#amount').val();
        var vatPercent = $('#VAT').val();
        var vatType = $('#VatTypeId').val();
        var vatTypeData = $('#VatTypeId option:selected').text();
        var unit = $('#unit option:selected').text();
        var unitId = $('#unit').val();
        var unitAmount = $('#Amount').val();
        var IsImport = $('#IsImport').is(":checked");
        var DiscountPerItem = $('#DiscountPerItem').val();
        var SupplementaryDutyPercent = $('#SupplementaryDutyPercent').val();
        var VDSAmount = $('#VDSAmount').val();
        var IsVatDeductedInSource = $('#IsVatDeductedInSource').is(":checked");
        var customDuty = $('#customDuty').val();
        var DefaultImportDutyPercent = $('#DefaultImportDutyPercent').val();
        var DefaultRegulatoryDutyPercent = $('#DefaultRegulatoryDutyPercent').val();
        var DefaultAdvanceTaxPercent = $('#DefaultAdvanceTaxPercent').val();
        var DefaultAdvanceIncomeTaxPercent = $('#DefaultAdvanceIncomeTaxPercent').val();
        if (productId > 0) {

            var canAdd = true;

            $.each(purchasOrder.PurchaseOrderDetailList,
                function (i, v) {
                    if (purchasOrder.PurchaseOrderDetailList[i].ProductId == productId) {
                        alert('Sorry! product Order Exists.');
                        canAdd = false;

                    }

                });
            if (!parseInt(item)) {
                alert('Number of item is invalid!');
                canAdd = false;
            }
//            if (!parseInt(amount)) {
//                alert('Amount of Price is invalid!');
//                canAdd = false;
//            }
            if (Number(amount) === NaN) {
                alert('Amount of Price is invalid!');
                canAdd = false;
            }
            if (Number(amount) === 0) {
                alert('Amount of Price should not be Zero');
                canAdd = false;
            }
            if (canAdd) {

                if (!purchasOrder.isNumber(vatPercent)) {
                    return alert("Input VAT %");
                }

                var vat = parseFloat(vatPercent) / 100;
                var atVat = parseFloat(DefaultAdvanceTaxPercent) / 100;
                var data = new Object();
                data.ProductId = productId;
                data.Quantity = item;
                data.UnitPrice = unitAmount;
                data.VATPercent = vatPercent;
                data.DiscountPerItem = DiscountPerItem;
                data.SupplementaryDutyPercent = SupplementaryDutyPercent;
                data.DefaultImportDutyPercent = DefaultImportDutyPercent;
                data.DefaultRegulatoryDutyPercent = DefaultRegulatoryDutyPercent;
                data.DefaultAdvanceTaxPercent = DefaultAdvanceTaxPercent;
                data.DefaultAdvanceIncomeTaxPercent = DefaultAdvanceIncomeTaxPercent;
                data.CustomDuty = customDuty;

                data.MeasurementUnitId = unitId;
                data.vatType = vatType;
                var vatValue = parseFloat(amount);
                vatValue = vatValue * (vat);
                var atValue = parseFloat(amount);
                atValue = atValue * (atVat);
                var total = parseFloat(amount) + vatValue;

                //data.totalPrice = total;
                //data.totalVat = vatValue;
                data.totalPrice = $('#totalValue').text();
                data.totalVat = $('#vatValue').text();
                data.ATVat = atValue;

                purchasOrder.PurchaseOrderDetailList.push(data);
                purchasOrder.UpperLimitPaymentMethod(data.totalPrice);
                purchasOrder.SetPaidAmount(data.totalPrice);
                if (!IsImport) {
                    if (IsVatDeductedInSource) {
                        var previousAmount = parseFloat(VDSAmount);
                        $('#VDSAmount').val(parseFloat(data.totalVat) + previousAmount);
                    }
                    var html = '<tr id="' +
                        data.ProductId +
                        '">' +
                        '<td>' +
                        purchasOrder.count +
                        '</td>' +
                        '<td>' +
                        productName +
                        '</td>' +
                        '<td>' +
                        item +
                        '</td>' +
                        '<td>' +
                        amount +
                        '</td>' +
                        '<td>' +
                        DiscountPerItem +
                        '</td>' +
                        '<td>' +
                        unit +
                        '</td>' +
                        '<td>' +
                        unitAmount +
                        '</td>' +
                        '<td>' +
                        SupplementaryDutyPercent +
                        '</td>' +
                        '<td>' +
                        vatTypeData +
                        '</td>' +
                        '<td>' +
                        vatPercent +
                        '</td>' +
                        '<td>' +
                        data.totalVat +
                        '</td>' +
                        '<td>' +
                        data.totalPrice +
                        '</td>' +
                        '<td><span onclick="Delete(' +
                        data.ProductId +
                        ')"  class="glyphicon glyphicon-minus btn-xs"></a></span></td>';

                    html += '</tr>';

                    $("table#gridTable > tbody").append(html);
                    purchasOrder.CalculateTotalPriceAndVat(total, vatValue);
                    purchasOrder.gridTableEmpty();
                    purchasOrder.count++;
                } else {

                    var previousAmount = parseFloat($('#AdvanceTaxPaidAmount').val());
                    $('#AdvanceTaxPaidAmount').val(atValue + previousAmount);

                    var html = '<tr id="' +
                        data.ProductId +
                        '">' +
                        '<td>' +
                        purchasOrder.count +
                        '</td>' +
                        '<td>' +
                        productName +
                        '</td>' +
                        '<td>' +
                        item +
                        '</td>' +
                        '<td>' +
                        amount +
                        '</td>' +
                        '<td>' +
                        DiscountPerItem +
                        '</td>' +
                        '<td>' +
                        unit +
                        '</td>' +
                        '<td>' +
                        unitAmount +
                        '</td>' +
                        '<td>' +
                        SupplementaryDutyPercent +
                        '</td>' +
                        //'<td>' +
                        //assessableValue +
                        //'</td>' +
                        '<td>' +
                        customDuty +
                        '</td>' +
                        '<td>' +
                        DefaultImportDutyPercent +
                        '</td>' +
                        '<td>' +
                        DefaultRegulatoryDutyPercent +
                        '</td>' +
                        '<td>' +
                        DefaultAdvanceTaxPercent +
                        '</td>' +
                        '<td>' +
                        DefaultAdvanceIncomeTaxPercent +
                        '</td>' +
                        '<td>' +
                        vatTypeData +
                        '</td>' +
                        '<td>' +
                        vatPercent +
                        '</td>' +
                        '<td>' +
                        vatValue +
                        '</td>' +
                        '<td>' +
                        total +
                        '</td>' +
                        '<td><span onclick="Delete(' +
                        data.ProductId +
                        ')"  class="glyphicon glyphicon-minus btn-xs"></a></span></td>';

                    html += '</tr>';

                    $("table#gridTable > tbody").append(html);
                    purchasOrder.CalculateTotalPriceAndVat(total, vatValue);
                    purchasOrder.gridTableEmpty();
                    purchasOrder.count++;
                }

            }
        }

    },


    delete: function (id) {
        var IsVatDeductedInSource = $('#IsVatDeductedInSource').is(":checked");
        for (var key in purchasOrder.PurchaseOrderDetailList) {
            var value = purchasOrder.PurchaseOrderDetailList[key].ProductId;
            if (purchasOrder.PurchaseOrderDetailList[key].ProductId == id) {
                purchasOrder.count -= 1;
                purchasOrder.UpperLimitPaymentMethod(-purchasOrder.PurchaseOrderDetailList[key].totalPrice);
                purchasOrder.CalculateTotalPriceAndVat(-purchasOrder.PurchaseOrderDetailList[key].totalPrice,
                    -purchasOrder.PurchaseOrderDetailList[key].totalVat);
                //purchasOrder.SetPaidAmount(-purchasOrder.PurchaseOrderDetailList[key].totalPrice);
                var previousAmount = $('#AdvanceTaxPaidAmount').val();
                var previousVDSAmount = $('#VDSAmount').val();
                $('#AdvanceTaxPaidAmount')
                    .val(parseFloat(previousAmount) - purchasOrder.PurchaseOrderDetailList[key].ATVat);
                if (IsVatDeductedInSource) {
                    $('#VDSAmount').val(parseFloat(previousVDSAmount) -
                        purchasOrder.PurchaseOrderDetailList[key].totalVat);
                }

            }
        }
        $.each(purchasOrder.PurchaseOrderDetailList,
            function (i, v) {
                if (purchasOrder.PurchaseOrderDetailList[i].ProductId == id) {

                    purchasOrder.PurchaseOrderDetailList.splice(i, 1);
                    if (purchasOrder.PurchasePaymenJson.length > 0) {
                        PaymentInfoReset();

                    }
                    purchasOrder.SetPaidAmount(0);
                }
                $("tr#" + id).remove().fadeOut();

            });

    },
    deleteDocument: function (id) {

        $.each(purchasOrder.ContentInfoJson,
            function (i, v) {
                if (purchasOrder.ContentInfoJson[i].rowCount == id) {

                    purchasOrder.ContentInfoJson.splice(i, 1);

                }
                $("tr#" + id).remove().fadeOut();

            });

    },
    deletePayment: function (id) {

        $.each(purchasOrder.PurchasePaymentJson,
            function (i, v) {
                if (purchasOrder.PurchasePaymentJson[i].rowCount == id) {

                    purchasOrder.PurchasePaymentJson.splice(i, 1);

                }
                $("tr#" + id).remove().fadeOut();

            });

    },

    calculate: function (value) {

        var unitAmount = $('#Amount').val();

        if (!purchasOrder.isNumber(unitAmount) || !(purchasOrder.isNumber(value))) {
            return;
        } else {
            $('#amount').val((unitAmount * value).toFixed(4));
            purchasOrder.calculateTotal();
        }

    },

    calculateUnitPrice: function (value) {
        var qty = $('#item').val();
        if (!purchasOrder.isNumber(qty) || !(purchasOrder.isNumber(value))) {

            return;
        } else {
            $("#Amount").val((value / qty).toFixed(4));
            purchasOrder.calculateTotal();
        }

    },
    calculateAmount: function (value) {
        var qty = $('#item').val();
        if (!purchasOrder.isNumber(qty) || !(purchasOrder.isNumber(value))) {

            return;
        } else {
            $("#amount").val((value * qty).toFixed(4));
            purchasOrder.calculateTotal();
        }

    },
    Save: function () {
        var IsImport = $('#IsImport').is(":checked");
        var VendorId = $('#VendorId').val();
        var VatChallanNo = $('#VatChallanNo').val();
        var VatChallanIssueDate = $('#VatChallanIssueDate').val();
        var VendorInvoiceNo = $('#VendorInvoiceNo').val();
        var InvoiceNo = $('#InvoiceNo').val();

        var AdvanceTaxPaidAmount = $('#AdvanceTaxPaidAmount').val();
        var ATPDate = $('#ATPDate').val();
        var ATPBankBranchId = $('#ATPBankBranchId').val();
        var ATPNbrEconomicCodeId = $('#ATPNbrEconomicCodeId').val();
        var ATPChallanNo = $('#ATPChallanNo').val();
        var CustomsAndVATCommissionarateId = $('#CustomsAndVATCommissionarateId').val();
        var PurchaseTypeId = 1;
        if (IsImport) {
            PurchaseTypeId = 2;
            VendorId = $('#VendorIdImport').val();
        }
        var files = $("#FileUpload").get(0).files;
        var Amount = $('#PurchaseAmount').val();
        var ExpectedDeliveryDate = $('#ExpectedDeliveryDate').val();
        var DeliveryDate = $('#DeliveryDate').val();
        var Iteams = $('#PurchaseIteams').val();
        var Vat = $('#PurchaseVat').val();
        var DiscountOnTotalPrice = $('#DiscountOnTotalPrice').val();
        var PurchaseReasonId = $('#PurchaseReasonId').val();
        var IsVatDeductedInSource = $('#IsVatDeductedInSource').is(":checked");
        var PaidAmount = $('#PaidAmount').val();
        var LcNo = $('#LcNo').val();
        var LcDate = $('#LcDate').val();
        var BillOfEntry = $('#BillOfEntry').val();
        var BillOfEntryDate = $('#BillOfEntryDate').val();
        var DueDate = $('#DueDate').val();
        var TermsOfLc = $('#TermsOfLc').val();
        var PoNumber = $('#PoNumber').val();
        var MushakGenerationId = $('#MushakGenerationId').val();
        var VDSAmount = $('#VDSAmount').val();
        //Bind Data for Purchase
        var data = new FormData();

        for (var i = 0; i < purchasOrder.PurchaseOrderDetailList.length; i++) {
            data.append('PurchaseOrderDetailList[' + i + '].ProductId',
                purchasOrder.PurchaseOrderDetailList[i].ProductId);
            data.append('PurchaseOrderDetailList[' + i + '].ProductVATTypeId',
                purchasOrder.PurchaseOrderDetailList[i].vatType);
            data.append('PurchaseOrderDetailList[' + i + '].Quantity',
                purchasOrder.PurchaseOrderDetailList[i].Quantity);
            data.append('PurchaseOrderDetailList[' + i + '].UnitPrice',
                purchasOrder.PurchaseOrderDetailList[i].UnitPrice);
            data.append('PurchaseOrderDetailList[' + i + '].DiscountPerItem',
                purchasOrder.PurchaseOrderDetailList[i].DiscountPerItem);
            data.append('PurchaseOrderDetailList[' + i + '].CustomDutyPercent',
                purchasOrder.PurchaseOrderDetailList[i].CustomDuty);
            data.append('PurchaseOrderDetailList[' + i + '].ImportDutyPercent',
                purchasOrder.PurchaseOrderDetailList[i].ImportDutyPercent);
            data.append('PurchaseOrderDetailList[' + i + '].RegulatoryDutyPercent',
                purchasOrder.PurchaseOrderDetailList[i].RegulatoryDutyPercent);
            data.append('PurchaseOrderDetailList[' + i + '].SupplementaryDutyPercent',
                purchasOrder.PurchaseOrderDetailList[i].SupplementaryDutyPercent);
            data.append('PurchaseOrderDetailList[' + i + '].VATPercent',
                purchasOrder.PurchaseOrderDetailList[i].VATPercent);
            data.append('PurchaseOrderDetailList[' + i + '].AdvanceTaxPercent',
                purchasOrder.PurchaseOrderDetailList[i].AdvanceTaxPercent);
            data.append('PurchaseOrderDetailList[' + i + '].AdvanceIncomeTaxPercent',
                purchasOrder.PurchaseOrderDetailList[i].AdvanceIncomeTaxPercent);
            data.append('PurchaseOrderDetailList[' + i + '].MeasurementUnitId',
                purchasOrder.PurchaseOrderDetailList[i].MeasurementUnitId);
        }
        //TODO
        for (var i = 0; i < purchasOrder.ContentInfoJson.length; i++) {
            data.append('ContentInfoJson[' + i + '].DocumentTypeId',
                purchasOrder.ContentInfoJson[i].DocumentTypeId);
            data.append('ContentInfoJson[' + i + '].UploadFile', purchasOrder.ContentInfoJson[i].UploadFile);
        }
        //TODO
        for (var i = 0; i < purchasOrder.PurchasePaymenJson.length; i++) {
            data.append('PurchasePaymenJson[' + i + '].PaymentMethodId',
                purchasOrder.PurchasePaymenJson[i].PaymentMethodId);
            data.append('PurchasePaymenJson[' + i + '].PaidAmount',
                purchasOrder.PurchasePaymenJson[i].PaidAmount);
            data.append('PurchasePaymenJson[' + i + '].ChequeNumber',
                purchasOrder.PurchasePaymenJson[i].ChequeNumber);
            data.append('PurchasePaymenJson[' + i + '].PaymentDate',
                purchasOrder.PurchasePaymenJson[i].PaymentDate);
        }
        data.append("VatChallanNo", VatChallanNo);
        data.append("VatChallanIssueDate", VatChallanIssueDate);
        data.append("VendorId", VendorId);
        data.append("VendorInvoiceNo", VendorInvoiceNo);
        if (IsImport) {
            data.append("AdvanceTaxPaidAmount", AdvanceTaxPaidAmount);
            data.append("ATPDate", ATPDate);
            data.append("ATPBankBranchId", ATPBankBranchId);
            data.append("ATPNbrEconomicCodeId", ATPNbrEconomicCodeId);
            data.append("ATPChallanNo", ATPChallanNo);

            data.append("CustomsAndVATCommissionarateId", CustomsAndVATCommissionarateId);
        }
        data.append("VDSAmount", VDSAmount);
        data.append("PurchaseDate", '2019-07-15');
        data.append("PurchaseTypeId", PurchaseTypeId);
        data.append("PurchaseReasonId", PurchaseReasonId);
        data.append("DiscountOnTotalPrice", DiscountOnTotalPrice);
        data.append("IsVatDeductedInSource", IsVatDeductedInSource);
        data.append("ExpectedDeliveryDate", ExpectedDeliveryDate);
        data.append("MushakGenerationId", 1); //TODO
        data.append("LcNo", LcNo);
        data.append("LcDate", LcDate);
        data.append("BillOfEntry", BillOfEntry);
        data.append("BillOfEntryDate", BillOfEntryDate);
        data.append("DueDate", DueDate);
        data.append("TermsOfLc", TermsOfLc);
        data.append("PoNumber", PoNumber);
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
                $("#loading").hide();

                if (result == false) {
                    alert("Please add atleast on purchase Details");
                } else {
                    window.location.href = returnURL;
                }
            },
            error: function (x, e) {
                $("#loading").hide();
                alert('Error');
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
                                var url = detailsAutoComplete;
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
                                $("#Amount").attr("readonly", false);
                                $("#item").attr("readonly", false);
                                $("#amount").attr("readonly", false);
                                $("#unit").attr("readonly", false);
                                $("#VAT").attr("readonly", false);
                                $("#ProductId").val(datum.id);
                                $('#Amount').val(datum.unitPrice);
                                $('#item').val('1');
                                $('#amount').val(datum.unitPrice);
                                $('#unit').val(datum.unit);
                                $('#SupplementaryDutyPercent').val(datum.supplimentaryDuty);
                                $('#DefaultImportDutyPercent').val(datum.defaultImportDutyPercent);
                                $('#DefaultRegulatoryDutyPercent').val(datum.defaultRegulatoryDutyPercent);
                                $('#DefaultAdvanceTaxPercent').val(datum.defaultAdvanceTaxPercent);
                                $('#DefaultAdvanceIncomeTaxPercent').val(datum.defaultAdvanceIncomeTaxPercent);
                                $('#VAT').val(datum.vat);
                                $('#VatTypeId').val(datum.productVATTypeId);
                                purchasOrder.unitAmount = datum.unitPrice;
                                purchasOrder.calculateTotal();
                                VatTypeChange();
                            });
                    $("#Product").focus();

                }
            }
            materialforrequsition.Initialize();
        }
    }
}

$(document).ready(function () {
    $('#purchaseForeign').validate({
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

            //InvoiceNo: "required",
            VendorId: {

                valueNotEquals: "0"
            },
            VendorIdImport: {

                valueNotEquals: "0"
            },
            VendorInvoiceNo: {
                required: true,
                maxlength: 50
            },
            PurchaseReasonId: {
                valueNotEquals: "0"
            },
            LcNo: {
                maxlength: 50
            },
            LcDate: {
                date: true,
            },
            VDSAmount: {
                maxlength: 8,
                number: true,
            },
            Discount: {
                maxlength: 8,
                number: true
            },
            DiscountPerItem: {
                maxlength: 8,
                number: true,
            },

            VatChallanNo: {
                required: true,
                maxlength: 50
            },
            VatChallanIssueDate: {
                required: true,
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
            PoNumber: {
                number: true,
            },
            ATPDate: {
                date: true,
            },
            AdvanceTaxPaidAmount: {
               // number: true,
               // checkdecimal:true
            },

            item: {
                required: true,
               // greaterThan: true,
                number: true
            },
            amount: {
                required: $("#amount").val() < 0,
                maxlength: 8,
                number: true
            },

            Product: {
                valueNotEquals: "0"
            },

            SupplementaryDutyPercent: {
                number: true
            },
            PaidAmount: {
                maxlength:8,
                number: true
            },
            VAT: {
                maxlength: 8,
                number: true
            },
            PaymentDate: {
                required: true,
                date: true
            }
        },
        messages: {

            //InvoiceNo: "Enter Invoice No"
            VendorId: {
                valueNotEquals: 'Select a Vendor'
            },
            VendorIdImport: {

                valueNotEquals: 'Select a Vendor'
            },
            PurchaseReasonId: {
                valueNotEquals: 'Select a Purchase reason'
            },
            LcNo: {
                maxlength: 'Value Can not more than 50 char'
            },
            LcDate: {
                date: 'Provide a valid Date',
            },
            Discount: {
                maxlength: 'Please add a valid number',
                number: 'Entrer Number'
            },
            VDSAmount: {
                maxlength: 'Please add a valid number',
                number: 'Entrer Number'
            },
            DiscountPerItem: {
                maxlength: 'Please add a valid number',
                number: 'Entrer Number'
            },
            VendorInvoiceNo: {
                required: 'THis item is required',
                maxlength: 'WorkOrderNo Can not be more than 50 char'
            },
            VatChallanNo: {
                required: 'This item is required',
                maxlength: 'VatChallanNo Can not more than 50 char'
            },
            VatChallanIssueDate: {

                date: 'Provide a valid Date',
            },
            BillOfEntry: {
                maxlength: ' BillOfEntry Can not more than 50 char'
            },
            BillOfEntryDate: {
                date: 'Provide a valid Date',
            },
            DueDate: {
                date: 'Provide a valid Date',
            },

            PoNumber: {
                number: 'Entrer Number'
            },
            ATPDate: {
                date: 'Provide a valid Date',
            },
            AdvanceTaxPaidAmount: {
                number: 'Entrer Number'
            },


            item: {
                required: 'Please provide Quantity',
                number: 'Entrer Number'
            },
            amount: {
                required: 'Please provide amount',
                maxlength: 'Please add a valid number',
                number: 'Entrer Number'
            },
            Product: {
                valueNotEquals: "0"
            },

            SupplementaryDutyPercent: {
                number: 'Entrer Number'
            },
            PaidAmount: {
                maxlength: 'Please add a valid number',
                number: 'Entrer Number'
            },
            VAT: {
                maxlength: 'Please add a valid number',
                number: 'Entrer Number'
            },
            PaymentDate: {
                required: 'Provide Payment date',
                date: 'Provide a valid Date'
            }

        },
        errorPlacement: function () {
            return false;
        }
    });

    $('#save').click(function () {
        $("#Product").rules('remove', 'required');
        $("#item").rules('remove', 'required');
        $("#amount").rules('remove', 'required');
        $("#PaymentDate").rules('remove', 'required');

        if ($('#IsImport:checked').length == 0) {
            $("#VendorId").rules('remove', 'required');
            $("#VendorIdImport").rules('remove', 'required');
            $("#VendorId").rules('add',
                {
                    valueNotEquals: '0'


                });
            $("#VendorInvoiceNo").rules('add', 'required');


        } else {
            $("#VendorId").rules('remove', 'required');
            $("#VendorId").rules('remove', 'valueNotEquals');
            $("#VendorIdImport").rules('add',
                {
                    valueNotEquals: '0'


                });
            $("#VendorInvoiceNo").rules('add', 'required');
        }
        $("#VatChallanNo").rules('add', 'required');
        $("#VendorInvoiceNo").rules('add', 'required');
        $("#PurchaseReasonId").rules('add', 'valueNotEquals');
        $("#PurchaseReasonId").rules('add', 'required');
        if ($("#purchaseForeign").valid()) {
            if (purchasOrder.PurchaseOrderDetailList.length != 0) {
                if (confirm('Are you sure?')) {

                    purchasOrder.Save();
                    return true;
                }
            }
            else {
                alert('Please add product');
            }



        } else {

            return false;
        }
   });

    $('#add').click(function () {

        $("#Product").rules('add', 'required');
        $("#item").rules('add', 'required');
        $("#amount").rules('add', 'required');
        $("#PaidAmount").rules('add', 'required');
        $("#PaymentDate").rules('remove', 'required');
        $("#VendorId").rules('remove', 'valueNotEquals');
        $("#VendorId").rules('remove', 'required');
        $("#VendorIdImport").rules('remove', 'valueNotEquals');
        $("#VatChallanNo").rules('remove', 'required');
        $("#VendorInvoiceNo").rules('remove', 'required');
        $("#PurchaseReasonId").rules('remove', 'valueNotEquals');
        $("#PurchaseReasonId").rules('remove', 'required');

        if ($("#purchaseForeign").valid()) {

            purchasOrder.add();
            return true;
        } else {
            return false;
        }

    });
    $('#addDocument').click(function () {

        purchasOrder.addDocument();
    });
    $('#addPayment').click(function () {
        $("#Product").rules('remove', 'required');
        $("#item").rules('remove', 'required');
        $("#amount").rules('remove', 'required');
        $("#PaidAmount").rules('add', 'required');
        $("#PaymentDate").rules('remove', 'required');
        $("#VendorId").rules('remove', 'valueNotEquals');
        $("#VendorId").rules('remove', 'required');
        $("#VendorIdImport").rules('remove', 'valueNotEquals');
        $("#VatChallanNo").rules('remove', 'required');
        $("#VendorInvoiceNo").rules('remove', 'required');
        $("#PurchaseReasonId").rules('remove', 'valueNotEquals');
        $("#PurchaseReasonId").rules('remove', 'required');
        if ($("#purchaseForeign").valid()) {

            purchasOrder.addPayment();

            return true;
        } else {
            return false;
        }

    });
    //$.validator.addMethod("greaterThan",
    //    function (value, element, param) {
    //        var val_a = parseInt($("#MaxSaleQty").val());
    //        return this.optional(element) || (parseInt(value) != 0 && parseInt(value) <= val_a);
    //    },
    //    "Please Check Quantity");
    $.validator.addMethod("valueNotEquals",
        function (value, element, arg) {
            return arg !== value;
        },
        "Value must not equal arg.");

    jQuery.validator.addMethod("checkdecimal", function (value, element) {
        return this.optional(element) || /^\d{0,4}(\.\d{0,2})?$/i.test(value);
    }, "You must include two decimal places");


        purchasOrder.product.productAutoComplete();
        //$("#ExpectedDeleveryDate").datepicker();
        //$("#ExpectedDeleveryDate").datepicker('option', 'dateFormat', 'dd M, yy');
        //$("#ATPDate").datepicker();
        //$("#ATPDate").datepicker('option', 'dateFormat', 'dd M, yy');
        //$('#VatChallanIssueDate').datetimepicker({
        //    maxDate: moment().startOf('minute').add(0, 'm')
        //});
        //$("#DeliveryDate").datepicker();
        //$("#DeliveryDate").datepicker('option', 'dateFormat', 'dd M, yy');
        //$("#PaymentDate").datepicker();
        //$("#PaymentDate").datepicker('option', 'dateFormat', 'dd M, yy');
        //$("#BillOfEntryDate").datepicker();
        //$("#BillOfEntryDate").datepicker('option', 'dateFormat', 'dd M, yy');
        //$("#DueDate").datepicker();
        //$("#DueDate").datepicker('option', 'dateFormat', 'dd M, yy');
        //$("#DeliveryDate").datepicker();
        //$("#DeliveryDate").datepicker('option', 'dateFormat', 'dd M, yy');
        //$("#LcDate").datepicker();
        //$("#LcDate").datepicker('option', 'dateFormat', 'dd M, yy');



    });

    function Delete(id) {
        purchasOrder.delete(id);
    }

    function DeleteDocument(id) {
        purchasOrder.deleteDocument(id);
    }

    function DeletePayment(id) {
        purchasOrder.deletePayment(id);
    }
    function PaymentInfoReset() {
        $("#paymentdt").empty();
        purchasOrder.PurchasePaymenJson = [];
    }
    function GridInfoReset() {
        $("#tblPurchaseOD").empty();
        $("#paymentdt").empty();
        $("#contentTabledt").empty();
        $('#purchaseForeign')[0].reset();
        //$("#ExpectedDeliveryDate").datepicker();
        //$("#ExpectedDeliveryDate").datepicker('option', 'dateFormat', 'dd M, yy');
        //$("#ATPDate").datepicker();
        //$("#ATPDate").datepicker('option', 'dateFormat', 'dd M, yy');
        //$("#DeliveryDate").datepicker();
        //$("#DeliveryDate").datepicker('option', 'dateFormat', 'dd M, yy');
        //$("#PaymentDate").datepicker();
        //$("#PaymentDate").datepicker('option', 'dateFormat', 'dd M, yy');
        //$("#BillOfEntryDate").datepicker();
        //$("#BillOfEntryDate").datepicker('option', 'dateFormat', 'dd M, yy');
        //$("#DueDate").datepicker();
        //$("#DueDate").datepicker('option', 'dateFormat', 'dd M, yy');
        //$("#DeliveryDate").datepicker();
        //$("#DeliveryDate").datepicker('option', 'dateFormat', 'dd M, yy');
        //$("#LcDate").datepicker();
        //$("#LcDate").datepicker('option', 'dateFormat', 'dd M, yy');
        purchasOrder.PurchaseOrderDetailList = [];
        purchasOrder.ContentInfoJson = [];
        purchasOrder.PurchasePaymenJson = [];
        purchasOrder.totalAmount = 0;
    };

    $("#item").keyup(function (evt) {

        var that = this,
            numberOfItem = $(this).val();
        var regex = new RegExp("^023-[0-9]{0,7}$");
        if (regex.test(numberOfItem)) {
            return evt.preventDefault();

        }

        purchasOrder.calculate(numberOfItem);

    });

    $("#amount").keyup(function () {
        var that = this,
            amount = $(this).val();
        purchasOrder.calculateUnitPrice(amount);

    });
    $("#Amount").keyup(function () {
        var that = this,
            amount = $(this).val();
        purchasOrder.calculateAmount(amount);

    });

    $("#PurchaseTypeId").change(function () {
        purchasOrder.typeId = $('#PurchaseTypeId').val();
        var value = $('#PurchaseTypeId').val();
        purchasOrder.purchaseForVDS(value);

    });
    $("#PaymentMethodId").change(function () {

        var id = $("#PaymentMethodId").val();
        if (id == 2) {
            $('.chequeSelection').show();
        } else {
            $('.chequeSelection').hide();
        }

    });
    $('#IsImport').change(function () {

        if ($('#IsImport:checked').length == 0) {
            purchasOrder.ProductVatForImport(false);
            purchasOrder.isChecked = $("#IsImport:checked").length;
            GridInfoReset();
            $('.p_element').hide();
            $('.vds_element').show();
            $('.details_element').hide();
            $("#IsImport").attr("checked", false);
        } else {
            purchasOrder.ProductVatForImport(true);
            purchasOrder.isChecked = $("#IsImport:checked").length;
            $('.p_element').show();
            $('.vds_element').hide();
            $('.details_element').show();
            GridInfoReset();
            $("#IsImport").attr("checked", purchasOrder.isChecked);
        }

    });
    $('#VatTypeId').change(function () {

        VatTypeChange();

    });
    $('#IsVatDeductedInSource').change(function () {

        if ($('#IsVatDeductedInSource:checked').length == 0) {
            GridInfoReset();
            purchasOrder.ProductVatForLocal(false);
            $("#IsVatDeductedInSource").attr("checked", false);

        } else {
            GridInfoReset();
            purchasOrder.ProductVatForLocal(true);
            $("#IsVatDeductedInSource").attr("checked", true);
        }

    });
    $('#VatTypeId').change(function () {

        purchasOrder.calculateTotal();

    });
    $("#VAT").keyup(function () {

        purchasOrder.calculateTotal();

    });
    function VatTypeChange() {
        var id = $('#VatTypeId').val();
        if (id == 3) {
            $('#VAT').val('15');
            purchasOrder.calculateTotal();
            $('#VAT').attr('readonly', 'true');

        } else if (id == 2) {
            $('#VAT').val('0');
            purchasOrder.calculateTotal();
            $('#VAT').attr('readonly', 'true');

        } else {
            $('#VAT').attr('readonly', false);

        }
    }

