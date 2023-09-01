(function () {
    angular.module("app-checkout")
        .controller("checkoutController", checkoutController);
    checkoutController.$inject = ['$http', '$filter', '$cookies', '$window', 'checkoutService', 'viewModelHelper'];
    function checkoutController($http, $filter, $cookies, $window, checkoutService, viewModelHelper) {
        var vm = this;
        vm.class = null;
        vm.class = 'loader loader-default is-active'; 
        ///getLoginUser();
        //vm.itemQuantityChange = itemQuantityChange;
        vm.sameShipping = sameShipping;
        vm.placeOrder = placeOrder;
        vm.shippingAddressSelect = shippingAddressSelect; 
        vm.newAddress = newAddress;
        vm.termChange = termChange;
        vm.disableForm = false;
        //vm.itemDetails = [{ name: 'Dell Laptop', id: 1, price: 56200, quantity: 1, total: 56200 }, { name: 'Transend HDD', id: 2, price: 8500, quantity: 1, total: 56200 }, { name: 'Samsung Monitor', id: 3, price: 12000, quantity: 1, total: 56200 }];
        vm.countries = [{ name: 'Nepal', id: 1}];
        vm.paymentMethod = [{ id: 1, methodName: "Please Select Term first" }];
        vm.address = {};
        vm.loginUser = null;
        vm.itemQuantity = 1;
        vm.itemPrice = 100;
        vm.itemTotal = 0;
        vm.cartItems = [];
        vm.total = 0;
        vm.grandTotal = 0;
          

        var th = ['', 'thousand', 'million', 'billion', 'trillion'];

        var dg = ['zero', 'one', 'two', 'three', 'four', 'five', 'six', 'seven', 'eight', 'nine'];
        var tn = ['ten', 'eleven', 'twelve', 'thirteen', 'fourteen', 'fifteen', 'sixteen', 'seventeen', 'eighteen', 'nineteen'];
        var tw = ['twenty', 'thirty', 'forty', 'fifty', 'sixty', 'seventy', 'eighty', 'ninety'];
        function toWords(s) {
            s = s.toString(); s =
            s.replace(/[\, ]/g, ''); if (s != parseFloat(s)) return 'not a number'; var x =
            s.indexOf('.'); if (x == -1) x = s.length; if (x > 15) return 'too big'; var n =
            s.split(''); var str = ''; var sk = 0; for (var i = 0; i < x; i++) {
                if((x - i) % 3 == 2) {
                    if (n[i] == '1') { str += tn[Number(n[i + 1])] + ' '; i++; sk = 1; }
                    else if (n[i] != 0) { str += tw[n[i] - 2] + ' '; sk = 1; }
                } else if (n[i] != 0) {
                    str +=
                    dg[n[i]] + ' '; if ((x - i) % 3 == 0) str += 'hundred '; sk = 1;
                } if ((x - i) % 3 == 1) {
                    if (sk)
                        str += th[(x - i - 1) / 3] + ' '; sk = 0;
                }
            } if (x != s.length) {
                var y = s.length; str +=
                'point '; for (var i = x + 1; i < y; i++) str += dg[n[i]] + ' ';
            } return str.replace(/\s+/g, ' ');
        }

        function getCompanyDetails(id) {
            checkoutService.getCompanyDetails(id).then(function (result) {
                if (result.success) {
                    vm.companyDetails = result.data;
                }
                else {
                    var message = {};
                    message.message = "Get company details, " + result.message + " in checkout.,";
                    viewModelHelper.bugReport(message,
                      function (result) {
                      });
                }
            });
        }

        //alert(toWords(2521));
        //var str = window.location.pathname;
        //console.log("str:"+str);
        //var cartId = str.substring(str.lastIndexOf("/") + 1, str.length);
        var cartId = $cookies.get('cartId');

        init(cartId);

        function placeOrder(addresses, item) {
            vm.class = null;
            vm.class = 'loader loader-default is-active';
            var date = new Date();
            var today = date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();
            vm.confirmOrderView = {};
            if (!addresses.id) {
                addresses.addressType = 2;
            }
            vm.confirmOrderView.cartId = vm.cartItems.id;
            vm.confirmOrderView.deliveryMethodId = item.deliveryMethodId; 
            vm.confirmOrderView.paymentMethodId = item.paymentMethodId.id;
            vm.confirmOrderView.paymentTermId = item.paymentTermId.id;
            vm.confirmOrderView.customerId = parseInt(vm.loginUser);
            vm.confirmOrderView.shippingAddressViewModel = addresses;
            vm.confirmOrderView.billingAddressId = vm.address.billingAddress.id;

            checkoutService.confirmOrder(vm.confirmOrderView)
                    .then(function (result) {
                        if (result.success) {
                        var pdfDocGenerator = generatePdf(addresses, today, vm.cartItems.shoppingCartDetails, vm.companyDetails, result.data.purchaseOrderNumber);
                        if (pdfDocGenerator) {
                            try {
                                pdfDocGenerator.getBase64(function (data) {
                                    vm.emailDetail = {};
                                    vm.emailDetail.fileBase64 = data;
                                    vm.emailDetail.billingAddressId = vm.address.billingAddress.id;

                                    checkoutService.sendEmailToCustomer(vm.emailDetail).then(function (result) {
                                        if (result.success) {
                                            
                                            $window.location.href('/placeorder');
                                        }
                                    });
                                    //vm.confirmOrderView.fileBase64 = data;
                                    //$window.location.href('/placeorder');
                                });
                            } catch (e) {
                                alert(e);
                            }
                                
                            } else {
                                var message = {};
                                message.message = "PDF generate, in checkout.,";
                                viewModelHelper.bugReport(message,
                                  function (result) {
                                  });
                            }
                            
                        } else {
                            var message = {};
                            message.message = "Confirm order, " + result.message + " in checkout.,";
                            viewModelHelper.bugReport(message,
                              function (result) {
                              });
                        }
                        vm.class = 'loader loader-default';
                    });
            
            

        }

        function calculate(item, type) {
            var value = '0';
            var total = '0';
            var discount = '0';
            var tax = '0';
            if (type === 1) {
                for (var i = 0; i < item.length; i++) {
                    //total amount calculation without discount and tax
                    if (item[i].productsRefByAssembledAndKit) {
                        for (var j = 0; j < item[i].productsRefByAssembledAndKit.length; j++) {
                            total = Number(total) + Number(item[i].productsRefByAssembledAndKit[j].quantity *
                                item[i].productsRefByAssembledAndKit[j].productCost);
                        }
                    } else {
                        total = Number(total) +  Number(item[i].quantity *
                                item[i].productCost);
                    }
                }
                value = total;
            }

            else if (type === 2) {
                for (var i = 0; i < item.length; i++) {
                        //calculate discount
                        if (item[i].productsRefByAssembledAndKit) {
                            for (var j = 0; j < item[i].productsRefByAssembledAndKit.length; j++) {
                                discount = Number(discount) + item[i].productsRefByAssembledAndKit[j].discount;
                            }
                        } else {
                            discount = Number(discount) + item[i].quantity *
                                    item[i].discount;
                        }
                    }
                value = discount;
            }

            else if (type === 3) {
                //value = Number(total) - Number(discount);
            }
            else if (type === 4) {
                for (var i = 0; i < item.length; i++) {
                    //calculate discount
                    if (item[i].productsRefByAssembledAndKit) {
                        for (var j = 0; j < item[i].productsRefByAssembledAndKit.length; j++) {
                            for (var k = 0; k < item[i].productsRefByAssembledAndKit[j].taxAndAmounts.length; k++) {
                                tax = Number(tax) + item[i].productsRefByAssembledAndKit[j].taxAndAmounts[k].taxAmount;
                            }
                        }
                    } else {
                        tax = Number(tax) + item[i].taxAmount;
                    }
                }
                value = tax;
            } else {
                
            }
            return value;
        }


        function generateTable(item) {
            var table = [];
            for (var i = 0; i < item.length; i++) {
                var productName = '';
                var productRate = '0';
                if (item[i].productsRefByAssembledAndKit) {
                    for (var j = 0; j < item[i].productsRefByAssembledAndKit.length; j++) {
                        productName += item[i].productsRefByAssembledAndKit[j].productName + ', ';
                        productRate += item[i].productsRefByAssembledAndKit[j].productCost + ',';
                    }
                    var productFullName = item[i].productName + '(' + productName + ')';
                    var row = [
                        { text: i + 1, alignment: 'center' }, { text: productFullName, alignment: 'left' },
                        { text: (item[i].quantity).toString(), alignment: 'center' }, { text: productRate.toString(), alignment: 'right' },
                        { text: item[i].totalCostWithTax.toString(), alignment: 'right' }
                    ];

                    table.push(row);
                } else {
                    var row = [
                        { text: i + 1, alignment: 'center' }, { text: item[i].productName, alignment: 'left' },
                        { text: (item[i].quantity).toString(), alignment: 'center' }, { text: item[i].productCost.toString(), alignment: 'right' },
                        { text: item[i].totalCostWithTax.toString(), alignment: 'right' }
                    ];

                    table.push(row);
                }
                
            }
            return table;
        }

        function generatePdf(addresses, today, items, comapnyDetails, purchaseOrderNumber) {
            var docDefinition = {
                content: [
                    {

                        style: 'tableExample',
                        table: {
                            headerRows: 1,
                            widths: ['60%', '*'],
                            body: [
                                    [
                                        {
                                            image: vm.logo,
                                            width: 100
                                        },
                                    {
                                        text:

                                               [
                                                { text: comapnyDetails.name + '\n', style: ['header'] },
                                                { text: comapnyDetails.address + '\n' },
                                                { text: 'Phone No: ' + comapnyDetails.phoneNumber + '\n' },
                                                { text: 'Email: ' + comapnyDetails.emailAddress + '\n' },
                                                { text: 'URL: ' + comapnyDetails.websiteUrl + '\n' }]
                                    }]

                            ]
                        },
                        layout: 'noBorders'
                    },

                  { text: 'INVOICE', style: ['invoice', 'alignmentcenter'] },
                  {
                      columns: ['VAT No: ' + vm.webSetting.sellerVatNo, { text: 'Invoice No: ' + purchaseOrderNumber, alignment: 'right' }], margin: [0, 20, 0, 0]
                  },
                  { text: 'Customer\'s Name: ' + addresses.firstName + ' ' + addresses.lastName, style: ['alignmentleft', 'bold'], margin: [0, 5, 0, 0] },
                    {
                        columns: ['Address: ' + addresses.city + ', ' + addresses.country + ', ' + addresses.addressLine1, { text: 'Tel: ' + addresses.telephone, alignment: 'right' }], margin: [0, 5, 0, 0]
                    },
                    {
                        columns: ['Customer\'s VAT no: ' + vm.address.vatNo, { text: 'Date: ' + today, alignment: 'right' }], margin: [0, 5, 0, 10]
                    },
                    {
                        table: {
                            headerRows: 1,
                            widths: ['5%', '40%', '5%', '35%', '15%'],
                            body: [
                              [{ text: 'SN', bold: true, alignment: 'center' }, { text: 'Particulars', bold: true, alignment: 'center' }, { text: 'Qty', bold: true, alignment: 'center' }, { text: 'Rate', bold: true, alignment: 'center' }, { text: 'Amount', bold: true, alignment: 'center' }]
                            ]
                        }
                    },
                    {
                        table: {
                            headerRows: 1,
                            widths: ['5%', '40%', '5%', '35%', '15%'],
                            body: generateTable(items)
                        }, layout: {
                            hLineWidth: function (i, node) {
                                return (i === 0 || i === node.table.body.length) ? 0.25 : 0;
                            },
                            vLineWidth: function (i, node) {
                                return (i === 0 || i === node.table.widths.length) ? .25 : .25;
                            },
                            hLineColor: function (i, node) {
                                return (i === 0 || i === node.table.body.length) ? 'black' : 'gray';
                            },
                            vLineColor: function (i, node) {
                                return (i === 0 || i === node.table.widths.length) ? 'black' : 'gray';
                            },
                        }
                    },
                    {
                        table: {
                            headerRows: 1,
                            widths: ['5%', '40%', '5%', '35%', '15%'],
                            body: [

                              [{ text: '', colSpan: 2, rowSpan: 4, alignment: 'center' }, {}, { text: 'TOTAL', bold: true, colSpan: 2, alignment: 'right' }, {}, { text: calculate(items, 1).toString(), alignment: 'center' }],
                              [{ text: '', colSpan: 2, alignment: 'center' }, {}, { text: 'DISCOUNT', bold: true, colSpan: 2, alignment: 'right' }, {}, { text: calculate(items, 2).toString(), alignment: 'center' }],
                              [{ text: '', colSpan: 2, alignment: 'center' }, {}, { text: 'SUB TOTAL', bold: true, colSpan: 2, alignment: 'right' }, {}, { text: (calculate(items, 1) - calculate(items, 2)).toString(), alignment: 'center' }],
                              [{ text: '', colSpan: 2, alignment: 'center' }, {}, { text: 'SUB TOTAL', bold: true, colSpan: 2, alignment: 'right' }, {}, { text: calculate(items, 4).toString().toString(), alignment: 'right' }],
                              [{ text: '', colSpan: 2, alignment: 'center' }, {}, { text: 'VAT %', bold: true, colSpan: 2, alignment: 'right' }, {}, { text: calculate(items, 4).toString(), alignment: 'center' }],
                              [{ text: 'CASH/CHEQUE', colSpan: 2, alignment: 'center', bold: true }, {}, { text: 'GRAND TOTAL', bold: true, colSpan: 2, alignment: 'right' }, {}, { text: ((calculate(items, 1) - calculate(items, 2)) + calculate(items, 4)).toString(), alignment: 'center' }]

                            ]
                        }
                    },
                    {
                        columns: [{ text: 'Amount in Words: ' + toWords(((calculate(items, 1) - calculate(items, 2)) + calculate(items, 4))) + 'only.', bold: true }], margin: [0, 20, 0, 5]
                    },
                    {
                        columns: [{ text: 'For Asia Infotech Pvt. Ltd.', bold: true, alignment: 'right' }], margin: [0, 70, 0, 0]
                    }

                ],

                styles: {
                    header: {
                        fontSize: 20,
                        bold: true
                    },
                    margintop: {
                        margin: [0, 0, 60, 0]
                    },
                    tablewidth: {
                        width: 500
                    },
                    alignmentright: {
                        alignment: 'right'
                    },
                    alignmentleft: {
                        alignment: 'left'
                    },
                    alignmentcenter: {
                        alignment: 'center'
                    },
                    bold: {
                        bold: true
                    },
                    invoice: {
                        fontSize: 22,
                        bold: true
                    }
                }
            }
            return pdfMake.createPdf(docDefinition);
            //pdfMake.createPdf(docDefinition).download();
        }
        
        //pdfMake.createPdf(docDefinition).download();

        function shippingAddressSelect(address) {
            vm.disableForm = false;
            checkoutService.getAddressById(address.id)
                    .then(function (result) {
                        if (result.success) {
                            vm.billingAddress = result.data;
                            vm.disableForm = true;
                           
                        } else {
                            var message = {};
                            message.message = "get address by id, " + result.message + " in checkout.,";
                            viewModelHelper.bugReport(message,
                              function (result) {
                              });
                        }
                    });
        }

        function getPaymentTerm() {
            checkoutService.getPaymentTerm()
                .then(function (result) {
                    if (result.success) {
                        vm.paymentTerm = result.data;
                    } else {
                        var message = {};
                        message.message = "get payment term, " + result.message + " in checkout.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                });
        }


        function termChange(term) {
            checkoutService.getMethodById(term.id)
                .then(function (result) {
                    if (result.success) {
                        if (result.data.length > 0) {
                            vm.paymentMethod = result.data;
                        } else {
                            vm.paymentMethod = [{ id: 1, methodName: "Please Select Term first" }];
                        }

                    } else {
                        var message = {};
                        message.message = "get method by id, " + result.message + " in checkout.,";
                        viewModelHelper.bugReport(message,
                          function (result) {
                          });
                    }
                });
        }

        function newAddress() {
            vm.billingAddress = {};
            vm.disableForm = false;
            vm.defaultShippingAddressId = null;
            vm.showAddShippingInformation = false;
        }

        function init(id) {
            checkoutService.getLoginUser()
                .then(function (result) {
                    if (result.data > 0) {
                        vm.loginUser = result.data;
                        //init(cartId);
                    } else {
                        vm.loginUser = 0;
                    }
                    checkoutService.updateCartItemsAfterLogin(Number(id), vm.loginUser).then(function (result) {
                        if (result.success) {
                            vm.cartItems = result.data;
                            for (var j = 0; j < vm.cartItems.shoppingCartDetails.length; j++) {
                                vm.cartItems.shoppingCartDetails[j].totalCostWithTax = 0;
                                var totaltax = 0;
                                    if (vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit) {
                                        for (var k = 0; k < vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit.length; k++) {
                                        
                                            for (var l = 0; l < vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k].taxAndAmounts.length; l++) {
                                                totaltax += vm.cartItems.shoppingCartDetails[j]
                                                    .productsRefByAssembledAndKit[k].taxAndAmounts[l].taxAmount;
                                            }
                                        var total = (vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k]
                                            .quantity *
                                            vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k]
                                            .productCost) + totaltax;
                                            
                                            vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k]
                                                .perItemCost = 0;
                                            vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k]
                                                .perItemCost += total;
                                            vm.cartItems.shoppingCartDetails[j].totalCostWithTax += vm.cartItems.shoppingCartDetails[j].productsRefByAssembledAndKit[k]
                                                .perItemCost;
                                        }
                                } else {
                                        var total = (vm.cartItems.shoppingCartDetails[j].quantity *
                                                vm.cartItems.shoppingCartDetails[j]
                                                .productCost) + totaltax;
                                        vm.cartItems.shoppingCartDetails[j].totalCostWithTax += total;
                                }
                                
                            }
                            checkoutService.getCustomerAddress(vm.loginUser)
                               .then(function (result) {
                                   if (result.success) {
                                       vm.address = result.data;
                                       for (var i = 0; i < result.data.addresses.length; i++) {
                                           if (result.data.addresses[i].isDefault === true) {
                                               vm.billingAddress = result.data.addresses[i];                                            
                                               
                                               vm.defaultShippingAddressId = result.data.addresses[i].id;
                                               vm.disableForm = true;
                                           }
                                       }
                                   } else {
                                       var message = {};
                                       message.message = "get customer address, " + result.message + " in checkout.,";
                                       viewModelHelper.bugReport(message,
                                         function (result) {
                                         });
                                   }
                               });
                            getCompanyDetails(result.data.companyId);
                            vm.class = 'loader loader-default';
                        } else {
                            var message = {};
                            message.message = "update cart, " + result.message + " in checkout.,";
                            viewModelHelper.bugReport(message,
                              function (result) {
                              });
                        }
                    });
                });
            checkoutService.crmLocation()
           .then(function (result) {
               if (result.success) {
                   vm.crmLocation = result.data;
               } else {
                   var message = {};
                   message.message = "get CRM location, " + result.message + " in checkout.,";
                   viewModelHelper.bugReport(message,
                     function (result) {
                     });
               }
           });
            checkoutService.getDeliveryMethod()
                   .then(function (result) {
                       if (result.success) {
                           vm.deliveryMethod = result.data;
                       } else {
                           var message = {};
                           message.message = "get delivery method, " + result.message + " in checkout.,";
                           viewModelHelper.bugReport(message,
                             function (result) {
                             });
                       }
                   });
            getPaymentTerm();
            getCompanyWebSetting();
            getCompanyLogo();
        }
        function getCompanyLogo() {
            checkoutService.getCompanyLogo().then(function(result) {
                if (result.success) {
                    if (result.data.imageUrl) {
                        //vm.logo = vm.crmLocation + "/" + result.data.imageUrl;
                        var xhr = new XMLHttpRequest();
                        xhr.onload = function () {
                            var reader = new FileReader();
                            reader.onloadend = function () {
                                vm.logo = reader.result;
                            }
                            reader.readAsDataURL(xhr.response);
                        };
                        xhr.open('GET', vm.crmLocation + "/" + result.data.imageUrl);
                        xhr.responseType = 'blob';
                        xhr.send();
                        
                    }
                    else {
                        vm.logo = 'data:image/png;base64,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';
                    }
                    
                } else {
                    var message = {};
                    message.message = "get company logo, " + result.message + " in checkout.,";
                    viewModelHelper.bugReport(message,
                      function (result) {
                      });
                }
            });
        }
        function convertFileToDataURLviaFileReader(url) {
            
        }
        function getCompanyWebSetting() {
            checkoutService.companyWebSetting().then(function(result) {
                if (result.success) {
                    vm.webSetting = result.data;
                } else {
                    var message = {};
                    message.message = "get company web setting, " + result.message + " in checkout.,";
                    viewModelHelper.bugReport(message,
                      function (result) {
                      });
                }
            });
        }
        vm.billingAddress = {};

        function sameShipping(address, event) {
            if (event.currentTarget.checked) {
                vm.billingAddress = address;
            } else {
                vm.billingAddress = {};
            }

        }

    }
})();