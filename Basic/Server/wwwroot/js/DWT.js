var DWObject = null;
Dynamsoft.DWT.AutoLoad = false;
Dynamsoft.DWT.ResourcesPath = "https://unpkg.com/dwt@18.0.0/dist";
//Dynamsoft.DWT.ProductKey = "your license key";

export function CreateDWT() {
    var height = 580;
    var width = 500;
    
    if (Dynamsoft.Lib.env.bMobile) {
        height = 350;
        width = 270;
    }

    Dynamsoft.DWT.Containers = [{ ContainerId: 'dwtcontrolContainer', Width: width, Height: height }];
    Dynamsoft.DWT.RegisterEvent('OnWebTwainReady', function () {
        DWObject = Dynamsoft.DWT.GetWebTwain('dwtcontrolContainer');
        DWObject.Viewer.height = height;
        DWObject.Viewer.width = width;
    });

    Dynamsoft.DWT.Load();
}

export function Scan() {
    if (DWObject) {
        DWObject.SelectSource(function () {
            DWObject.OpenSource();
            DWObject.AcquireImage();
        },
            function () {
                console.log("SelectSource failed!");
            }
        );
    }
}

export function LoadImage() {
    if (DWObject) {
        DWObject.LoadImageEx('', 5,
            function () {
                console.log('success');
            },
            function (errCode, error) {
                alert(error);
            }
        );
    }
}

export function Save() {
    DWObject.IfShowFileDialog = true;
    // The path is selected in the dialog, therefore we only need the file name
    DWObject.SaveAllAsPDF("Sample.pdf",
        function () {
            console.log('Successful!');
        },
        function (errCode, errString) {
            console.log(errString);
        }
    );
}

export function isDesktop() {
    if (Dynamsoft.Lib.env.bMobile) {
        return false;
    } else {
        return true;
    }
}