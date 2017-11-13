import React from 'react'
import PropTypes from 'prop-types';

import InputPane from './InputPane';
import ListView from './ListView';
import Button from './Button';

import { VIEW_INPUT } from '../constants/viewTypes';
import _ from '../../stylesheets/components/_MainPane.scss';

export default class MainPane extends React.Component {
    render() {
        const { view, fetchTextEntry, changeView, textEntryTokens } = this.props;        
        const isInput = view === VIEW_INPUT;
        const title = isInput ? "Start" : "Results";
        const leftPane = isInput ? (
            // Input View
            <InputPane fetchTextEntry={fetchTextEntry} changeView={changeView} />
        ) : (
            // TODO: render highlighted sentences
            <div>
                <Button 
                    handleClick={() => changeView(VIEW_INPUT)} 
                    text="View Input"
                />
            </div>
        );
        const rightPane = isInput ? (
            // TODO: render feed
            <div />
        ) : (
            // Result View
            <div>
                <ListView entries={textEntryTokens} />
            </div>
        );
        return (
            <div>
                <span className="ms-font-su ms-fontColor-themePrimary">{title}</span>
                <div className="container">
                    <div className="row">
                        <div className="col-sm-6 col-md-6 col-lg-6">
                            {leftPane}
                        </div>
                        <div className="col-sm-6 col-md-6 col-lg-6">
                            {rightPane}
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

MainPane.propTypes = {
    view: PropTypes.string.isRequired,
    changeView: PropTypes.func.isRequired,
    fetchTextEntry: PropTypes.func.isRequired,
    textEntryTokens: PropTypes.array.isRequired
}