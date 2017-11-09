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
            <div>
                <InputPane addFact={this.props.addFact} />
                <ListView facts={this.props.facts} />
                <button 
                    className={"change-view-button"}
                    onClick={() => this.props.changeView(VIEW_RESULT)}
                >
                    View Results
                </button>
            </div>
        ) : (
            // Result View
            <div>
                <p> Results </p>
                <button 
                    className={"change-view-button"}
                    onClick={() => this.props.changeView(VIEW_INPUT)}>
                    View Input
                </button>
            </div>
        );
        return (
            <div>
                <h2> Placeholder!! </h2>
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