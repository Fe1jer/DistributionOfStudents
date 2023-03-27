export default function FacultyCardPreloader() {
    return (
        <div className="col-xxl-3 col-sm-6 col-md-4 pt-2 pb-2">
            <div className="card shadow-sm">
                <div className="scale card-img__faculty">
                    <svg className="bd-placeholder-img card-img-top" width="100%" role="img" aria-label="Placeholder" preserveAspectRatio="xMidYMid slice" focusable="false">
                        <rect width="100%" height="100%" fill="#868e96">
                        </rect>
                    </svg>
                </div>
                <div className="card-body">
                    <div className="box">
                        <div className="h-auto">
                            <h4 className="placeholder-glow"><span className="placeholder w-25"></span></h4>
                            <p className="placeholder-glow"><span className="placeholder col-10 bg-success"></span><br></br><span className="placeholder col-6 bg-success"></span></p>
                        </div>
                    </div>
                    <div className="d-flex justify-content-between align-items-center">
                        <button type="button" className="btn btn-outline-success disabled placeholder col-3" ></button>
                    </div>
                </div>
            </div>
        </div >
    );
}