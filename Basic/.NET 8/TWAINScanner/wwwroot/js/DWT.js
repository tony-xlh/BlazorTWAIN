let DWObject = null;
Dynamsoft.DWT.AutoLoad = false;
Dynamsoft.DWT.ResourcesPath = "https://unpkg.com/dwt@18.5.0/dist";
//Dynamsoft.DWT.ProductKey = "your license key";

function CreateDWT() {
    console.log("create dwt");
    let height = 580;
    let width = 500;

    Dynamsoft.DWT.Containers = [{ ContainerId: 'dwtcontrolContainer', Width: width, Height: height }];
    Dynamsoft.DWT.RegisterEvent('OnWebTwainReady', function () {
        DWObject = Dynamsoft.DWT.GetWebTwain('dwtcontrolContainer');
        DWObject.Viewer.height = height;
        DWObject.Viewer.width = width;
    });

    Dynamsoft.DWT.Load();
}

function Scan() {
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

function LoadImage() {
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

function Save() {
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
