import Button from 'react-bootstrap/Button';
import Card from 'react-bootstrap/Card';
import Col from 'react-bootstrap/Col';

export default function FacultyCardPreloader() {
    return (
        <Col xxl="3" sm="6" md="4" className="pt-2 pb-2">
            <Card className="shadow-sm">
                <div className="scale card-img__faculty">
                    <svg className="bd-placeholder-img card-img-top" width="100%" role="img" aria-label="Placeholder" preserveAspectRatio="xMidYMid slice" focusable="false">
                        <rect width="100%" height="100%" fill="#868e96">
                        </rect>
                    </svg>
                </div>
                <Card.Body>
                    <div className="box">
                        <div className="h-auto">
                            <Card.Title as="h4" className="placeholder-glow"><span className="placeholder w-25"></span></Card.Title>
                            <Card.Text className="placeholder-glow">
                                <span className="placeholder col-10 bg-success"></span><br></br><span className="placeholder col-6 bg-success"></span>
                            </Card.Text >
                        </div>
                    </div>
                    <div className="d-flex justify-content-between align-items-center">
                        <Button variant="outline-success" className="disabled placeholder col-3" ></Button>
                    </div>
                </Card.Body>
            </Card>
        </Col >
    );
}