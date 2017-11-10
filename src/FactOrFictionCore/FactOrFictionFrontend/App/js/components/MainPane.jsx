import React from 'react'
import PropTypes from 'prop-types';
import InputPane from './InputPane';
import ListView from './ListView';
import { VIEW_INPUT, VIEW_RESULT } from '../constants/viewTypes';
import _ from '../../stylesheets/components/_MainPane.scss';

export default class MainPane extends React.Component {
    render() {
        const currentPane = this.props.view === VIEW_INPUT ? (
            // Input View
            <div className="container">
                <div className="row">
                    <div className="input-pane col-sm-6 col-md-6 col-lg-6">
                        <InputPane addFact={this.props.addFact} />
                    </div>
                    <div className="col-sm-6 col-md-6 col-lg-6">
                        <ListView facts={this.props.facts} />
                        <button 
                            className={"change-view-button ms-Button"}
                            onClick={() => this.props.changeView(VIEW_RESULT)}
                        >
                            <span className="ms-Button-icon">
                                <i className="ms-Icon ms-Icon--plus" />
                            </span>
                            <span className="ms-Button-label">View Result</span>
                        </button>
                    </div>
                </div>
           </div>
        ) : (
            // Result View
            <div className="result-view">
                <p> Results </p>
                <button 
                    className={"change-view-button ms-Button"}
                    onClick={() => this.props.changeView(VIEW_INPUT)}>
                    View Input
                </button>
            </div>
        );
        return (
            <div>
                <span className="ms-font-su ms-fontColor-themePrimary">Input Some Text</span>
                {currentPane}
            </div>
        )
    }
}

MainPane.propTypes = {
    view: PropTypes.string.isRequired,
	facts: PropTypes.array.isRequired,
    addFact: PropTypes.func.isRequired,
    changeView: PropTypes.func.isRequired    
}