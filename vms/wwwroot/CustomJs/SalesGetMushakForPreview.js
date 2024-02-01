var getMushakForPreview = (orgName, binNo, address, issueAddress, customerName, customerBin, customerAddress, shippingAddress, driverName, driverMobileNo, vehicleType, vehicleRegistrationNo, buyOrderNo, soNo, invoiceNo, salesDetails) => {
    let salesDetailsText = ``;
    let draftDetailSl = 0;
    salesDetails.forEach(item => {
        draftDetailSl++;
        salesDetailsText += `
                <tr>
                  <td class="serial-cell">${draftDetailSl}</td>
                  <td class="name-cell">${item.productName}</td>
                  <td class="unit-name-cell">${item.measurementUnitName}</td>
                  <td class="amount-quantity-cell">${item.quantityToDisplay}</td>
                  <td class="amount-quantity-cell">${item.unitPriceToDisplay}</td>
                  <td class="amount-quantity-cell">${item.totalPriceToDisplay}</td>
                  <td class="amount-quantity-cell">${item.sdPercentToDisplay}</td>
                  <td class="amount-quantity-cell">${item.sdValueToDisplay}</td>
                  <td class="amount-quantity-cell">${item.vatPercentToDisplay}</td>
                  <td class="amount-quantity-cell">${item.totalVatToDisplay}</td>
                  <td class="amount-quantity-cell">${item.totalPriceWithVatAfterDiscountToDisplay}</td>
                </tr>
`;
    });

    let draftText = `
    <div class="report-section">
      <div class="report-margin" id="printableArea">
        <div class="row">
          <div class="col-2"></div>

          <div class="col-8 report-header mt-3">
            <div>GOVERNMENT OF THE PEOPLE&#x27;S REPUBLIC OF BANGLADESH</div>
            <div class="sub-header">NATIONAL BOARD OF REVENUE</div>
          </div>
          <div class="col-2">
            <div class="mushak-name">Mushak- 6.3</div>
          </div>

          <div class="col-12 report-header">
            <div class="sub-header">Tax Invoice</div>
            <div class="sub-header">
              [Note clause (c) and clause (f) of sub-rule (1) of rule 40]
            </div>
          </div>
          <div class="col-12 text-center mt-2">
            <div>Name of the registered person: ${orgName}</div>
            <div>BIN of the registered person: ${binNo}</div>
            <div class="company-other-info">
              Address of the registered person: Madina Tower, 464/1 DIT Road
              West Rampura (Opposite Rampura Tv Center), Dhaka 1219 , Bangladesh
            </div>
            <div>
              Address of invoice issue: Madina Tower, 464/1 DIT Road West
              Rampura (Opposite Rampura Tv Center), Dhaka 1219 , Bangladesh
            </div>
          </div>
          <div class="col-8 mt-3">
            <div style="margin-top: 3px">
              Customer Name: ABC Group Bangladesh
            </div>
            <div style="margin-top: 3px">Customer BIN: 000000203-0002</div>
            <div style="margin-top: 3px">Customer Address: Gulshan, Dhaka</div>
            <div style="margin-top: 3px">
              Shipping Address: 238 Bangshal Rd, Dhaka 1100
            </div>

            <div>Driver Name : Md. Jamal</div>
            <div>Driver Mobile No. : 01708588288</div>
            <div>Vehicle Type : Covered Van</div>
            <div>Vehicle Registration No. : MM H 00283</div>
          </div>

          <div class="col-4 mt-3">
            <div>Buy Order No./SO No.:</div>
            <div>PO No. :</div>

            <div style="margin-top: 3px">Invoice No.: 0000000002</div>
            <div style="margin-top: 3px">Invoice Date:</div>
            <div>Sales Date : 17/10/2022</div>
            <div>Challan No. : 0000000002</div>
            <div style="margin-top: 3px">Issue Date: 17/10/2022</div>
            <div style="margin-top: 3px">Issue Time: 04:52:33 PM</div>
          </div>

          <div class="col-12 mt-3 copy-watermark">
            <table class="table table-bordered">
              <tbody>
                <tr class="text-center">
                  <td>SL</td>

                  <td>
                    Description of product or service (with brand name if
                    applicable)
                  </td>
                  <td>Unit of Supply</td>
                  <td>Quantity</td>
                  <td>Unit Price<sup>1</sup> (Taka)</td>
                  <td>Total Price<sup>1</sup> (Taka)</td>
                  <td>SD Rate</td>
                  <td>SD Amount (Taka)</td>
                  <td>Value Added Tax Rate / Specific Tax</td>
                  <td>Amount of Value Added Tax/Specified Tax</td>
                  <td>Price Including All Kinds of Duties and Taxes</td>
                </tr>
                <tr class="text-center">
                  <td>(1)</td>
                  <td>(2)</td>
                  <td>(3)</td>
                  <td>(4)</td>
                  <td>(5)</td>
                  <td>(6)</td>
                  <td>(7)</td>
                  <td>(8)</td>
                  <td>(9)</td>
                  <td>(10)</td>
                  <td>(11)</td>
                </tr>
                ${salesDetailsText}
                <tr>
                  <td colspan="5" class="total-text-cell text-right">Total</td>
                  <td class="amount-quantity-cell">50,500.00</td>
                  <td class="amount-quantity-cell"></td>
                  <td class="amount-quantity-cell">0.00</td>
                  <td class="amount-quantity-cell"></td>
                  <td class="amount-quantity-cell">7,575.00</td>
                  <td class="amount-quantity-cell">58,075.00</td>
                </tr>
                
              </tbody>
            </table>
          </div>

          <div class="col-12" style="margin-top: 100px; padding-left: 20px">
            <div>
              Name of the person in charge of the Organization: Sabbir Ahmed
              Osmani
            </div>
            <div>Designation: Sr. Officer, Finance &amp; Accounts</div>
            <div class="mt-3 mb-3">
              Signature:
            </div>
            <div class="mb-4 mt-3">Seal:</div>

            <div>
              * In case of deductible supply at source, the form will be
              considered as integrated tax invoice and tax deduction certificate
              at source and it will be applicable for deductible supply at
              source.
            </div>
          </div>

          <div class="col-12 mt-2">
            <div class="row">
              <div class="col-3">
                <hr class="black-hr" />
                <sup>1</sup>Prices excluding all types of taxes
              </div>
            </div>
          </div>

          <div class="col-12 mt-3">
            <div class="font-weight-semi-bold">Remarks :</div>
            <div class="ps-4">According to SRO 1234 Vat is 5%</div>
          </div>
        </div>
        <div class="print-footer">
          <div class="text-center">
            ** This document is system-generated. No signature is required. **
          </div>
        </div>
      </div>
    </div>
`;
    return draftText;
}