import React from 'react';
import Card from 'react-bootstrap/Card';

import $ from 'jquery';
import 'jquery-ui/ui/widgets/sortable';

import "../../css/sortable.css"

export default function Sortable({ onChange, data }) {
    const sortableRef = React.useRef();

    React.useEffect(() => {
        $(".one")
            .addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all");
        if (sortableRef && sortableRef.current) {
            var $node = $(sortableRef.current);
            $node.sortable({
                connectWith: ".sortableCard",
                handle: ".one-content",
                placeholder: "one-placeholder card",
                update: (event, ul) => onChange(event, ul),
            });
        }
    }, []);

    const renderItems = () => {
        return data.map((item, i) => (
            <Card key={i} className="one shadow-sm my-1">
                <div className="one-content d-flex h-100" role="button">
                    <strong className="alert-secondary py-2 px-1"><span className="align-middle">№</span><span className="align-middle index">{i + 1}</span></strong>
                    <div className="ms-2 py-2 w-100"><span className="align-middle item">{item}</span></div>
                    <div className="align-self-center">
                        <svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" fill="currentColor" className="bi bi-list align-middle" viewBox="0 0 16 16">
                            <path fillRule="evenodd" d="M2.5 12a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5zm0-4a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5zm0-4a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5z" />
                        </svg>
                    </div>
                </div>
            </Card>
        ));
    }
    return (
        <Card ref={sortableRef} className="sortableCard">
            {renderItems()}
        </Card>
    );

};