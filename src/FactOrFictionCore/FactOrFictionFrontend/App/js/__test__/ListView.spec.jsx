import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import ListView from '../components/ListView.jsx';

describe('ListView', () => {

    let state;    
    let listView;
    
    beforeEach(() => {
        state = ["hi"]
        listView = mount(<ListView facts={state} />);
    });

    it('ListView requires facts prop', () => {
        expect(listView.props().facts).toBeDefined();
    });

    it('ListView renders nested components == 1', () => {
        expect(listView.find('FactCard').length).toEqual(1);
    });

    it('ListView renders nested components == number of facts', () => {
        state = []
        listView = mount(<ListView facts={state} />);
        expect(listView.find('FactCard').length).toEqual(0);
    });

    it('ListView renders nested components == number of facts', () => {
        state = ["hi", "hello"]
        listView = mount(<ListView facts={state} />);
        expect(listView.find('FactCard').length).toEqual(2);
    });
});